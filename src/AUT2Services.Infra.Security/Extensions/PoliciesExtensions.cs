using AUT2Services.Domain.Enumerations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;

namespace AUT2Services.Infra.Security.Extensions;

public static class PoliciesExtensions
{
    public static IServiceCollection AddPoliciesServices(this IServiceCollection services)
    {
        services.AddAuthorization(opt =>
        {
            opt.AddPolicy(
                EnumPolicyMaster.ADMINISTRADOR,
                policy => policy.RequireAssertion(context =>
                    HasAdministracionRole(context, EnumRolesBase.ADMINISTRADOR)));

            opt.AddPolicy(
                EnumPolicyMaster.ADMINISTRADOR_ENTIDAD,
                policy => policy.RequireAssertion(context =>
                    HasAdministracionRole(
                        context,
                        EnumRolesBase.ADMINISTRADOR,
                        EnumRolesBase.ADMINISTRADOR_ENTIDAD)));

            opt.AddPolicy(
                EnumPolicyMaster.ADMINISTRADOR_ENTIDAD_UNIDAD,
                policy => policy.RequireAssertion(context =>
                    HasAdministracionRole(
                        context,
                        EnumRolesBase.ADMINISTRADOR,
                        EnumRolesBase.ADMINISTRADOR_ENTIDAD,
                        EnumRolesBase.ADMINISTRADOR_UNIDAD)));

            opt.AddPolicy(
                EnumPolicyMaster.ADMINISTRADOR_ENTIDAD_UNIDAD_USUARIO,
                policy => policy.RequireAssertion(context =>
                    HasAdministracionRole(
                        context,
                        EnumRolesBase.ADMINISTRADOR,
                        EnumRolesBase.ADMINISTRADOR_ENTIDAD,
                        EnumRolesBase.ADMINISTRADOR_UNIDAD,
                        EnumRolesBase.USUARIO)));

            opt.AddPolicy(
                EnumPolicyMaster.VALIDA_ASIGNACION_ROLES,
                policy => policy.RequireAssertion(context =>
                    HasAdministracionRole(
                        context,
                        EnumRolesBase.ADMINISTRADOR,
                        EnumRolesBase.VALIDA_ASIGNACION_ROLES)));

            opt.AddPolicy(
                EnumPolicyMaster.VALIDA_ENRROLAMIENTO,
                policy => policy.RequireAssertion(context =>
                    HasAdministracionRole(
                        context,
                        EnumRolesBase.ADMINISTRADOR,
                        EnumRolesBase.VALIDA_ENRROLAMIENTO)));

            opt.AddPolicy(
                EnumPolicyMaster.USUARIO_LOGUEADO,
                policy => policy.RequireAuthenticatedUser());
        });

        return services;
    }

    private static bool HasAdministracionRole(AuthorizationHandlerContext context, params string[] allowedRoles)
    {
        var legacyClaimType = EnumProcesosBase.ADMINISTRACION;
        var currentRoleClaimType = $"{EnumProcesosBase.ADMINISTRACION}{EnumPartialBusinessClaimTypes._ROL}";

        return context.User.Claims.Any(claim =>
            (claim.Type == ClaimTypes.Role
             || claim.Type == legacyClaimType
             || claim.Type == currentRoleClaimType)
            && allowedRoles.Contains(claim.Value, StringComparer.Ordinal));
    }
}
