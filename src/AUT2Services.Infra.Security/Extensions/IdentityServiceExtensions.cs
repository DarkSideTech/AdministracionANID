using AUT2Services.Domain.Core.Models;
using AUT2Services.Domain.Security.Entities;
using AUT2Services.Infra.Data.Context;
using AUT2Services.Infra.Security.Enumerations;
using AUT2Services.Infra.Security.Interfaces;
using AUT2Services.Infra.Security.Models;
using AUT2Services.Infra.Security.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.RateLimiting;

namespace AUT2Services.Infra.Security.Extensions;

public class IdentityServiceExtensions
{
    public static void AddIdentityServices(WebApplicationBuilder builder)
    {
        builder.Services.Configure<ZendeskSendTicketOptions>(
            builder.Configuration.GetSection(ZendeskSendTicketOptions.ZendeskTicketOptionsKey));

        builder.Services.AddIdentityCore<Usuario>(opt =>
            {
                opt.User.RequireUniqueEmail = true;
                opt.SignIn.RequireConfirmedEmail = true;
                opt.Password.RequireNonAlphanumeric = false;
            })
            .AddRoles<Rol>()
            .AddEntityFrameworkStores<AUT2ServicesContext>()
            .AddDefaultTokenProviders();

        builder.Services
            .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
            .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateIssuerSigningKey = true,
                        ValidateLifetime = true,
                        ValidIssuer = builder.Configuration["JwtOptions:Issuer"],
                        ValidAudience = builder.Configuration["JwtOptions:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtOptions:Secret"]!)),
                        ClockSkew = TimeSpan.Zero
                    };
                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            if (context.Request.Cookies.TryGetValue(EnumAuthCookieNames.AccessToken, out var accessToken))
                            {
                                context.Token = accessToken;
                            }

                            return Task.CompletedTask;
                        },
                        OnTokenValidated = async context =>
                        {
                            var userId = context.Principal?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                            var sessionId = context.Principal?.FindFirst(JwtRegisteredClaimNames.Sid)?.Value;
                            var securityStamp = context.Principal?.FindFirst(EnumTokenValidationClaims.SecurityStamp)?.Value;

                            var sessionValidator = context.HttpContext.RequestServices.GetRequiredService<ISessionValidationService>();
                            var isValid = await sessionValidator.IsSessionValidAsync(userId, sessionId, securityStamp, context.HttpContext.RequestAborted);

                            if (!isValid)
                            {
                                context.Fail("Invalid or revoked session.");
                            }
                        }
                    };
                });

        builder.Services.AddAuthorization();

        builder.Services.AddRateLimiter(options =>
        {
            options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

            options.AddPolicy("ResendConfirmationEmail", httpContext =>
            {
                var settings = httpContext.RequestServices
                    .GetRequiredService<Microsoft.Extensions.Options.IOptions<EmailValidationOptions>>()
                    .Value;

                var remoteIp = httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";

                return RateLimitPartition.GetFixedWindowLimiter(
                    partitionKey: remoteIp,
                    factory: _ => new FixedWindowRateLimiterOptions
                    {
                        PermitLimit = Math.Max(1, settings.ResendRequestsPerWindow),
                        Window = TimeSpan.FromMinutes(Math.Max(1, settings.ResendWindowMinutes)),
                        QueueLimit = 0,
                        AutoReplenishment = true
                    });
            });
        });

        builder.Services.AddHostedService<ExpiredRefreshTokenCleanupService>();
    }
}
