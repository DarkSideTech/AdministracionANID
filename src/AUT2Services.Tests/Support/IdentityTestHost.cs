using AUT2Services.Domain.Security.Entities;
using AUT2Services.Infra.DataTrazabilidad.Auditing;
using AUT2Services.Infra.Data.Context;
using AUT2Services.Infra.Security.Interfaces;
using AUT2Services.Infra.Security.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;

namespace AUT2Services.Tests.Support;

internal sealed class IdentityTestHost : IDisposable
{
    public AUT2ServicesContext DbContext { get; }
    public UserManager<Usuario> UserManager { get; }
    public ISecurityTraceabilityService SecurityTraceabilityService { get; }

    public IdentityTestHost(string databaseName)
    {
        var clock = new TestClock(DateTimeOffset.Parse("2026-04-16T10:00:00Z"));
        var auditBuffer = new InMemoryAuditBuffer();
        var auditExecutionContext = new ScopedAuditExecutionContext(clock);
        var auditSerializer = new SystemTextJsonAuditSerializer();
        var options = new DbContextOptionsBuilder<AUT2ServicesContext>()
            .UseInMemoryDatabase(databaseName)
            .ConfigureWarnings(warnings => warnings.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;

        DbContext = new AUT2ServicesContext(
            options,
            new StubMediatorHandler(),
            auditBuffer,
            auditExecutionContext,
            new DefaultAuditDeltaBuilder(auditSerializer),
            auditSerializer,
            clock);
        SecurityTraceabilityService = new SecurityTraceabilityService(auditBuffer);

        var store = new UserStore<Usuario, Rol, AUT2ServicesContext>(DbContext);
        var identityOptions = Options.Create(new IdentityOptions
        {
            Password =
            {
                RequireDigit = true,
                RequiredLength = 6,
                RequireLowercase = true,
                RequireUppercase = true,
                RequireNonAlphanumeric = false
            },
            User =
            {
                RequireUniqueEmail = true
            }
        });

        UserManager = new UserManager<Usuario>(
            store,
            identityOptions,
            new PasswordHasher<Usuario>(),
            [new UserValidator<Usuario>()],
            [new PasswordValidator<Usuario>()],
            new UpperInvariantLookupNormalizer(),
            new IdentityErrorDescriber(),
            new ServiceCollection().BuildServiceProvider(),
            NullLogger<UserManager<Usuario>>.Instance);
    }

    public void Dispose()
    {
        UserManager.Dispose();
        DbContext.Dispose();
    }
}
