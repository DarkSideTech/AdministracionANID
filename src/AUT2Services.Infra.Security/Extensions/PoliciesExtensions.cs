using AUT2Services.Domain.Enumerations;
using Microsoft.Extensions.DependencyInjection;

namespace AUT2Services.Infra.Security.Extensions;

public static class PoliciesExtensions
{
    public static IServiceCollection AddPoliciesServices(this IServiceCollection services)
    {
        services.AddAuthorization(opt =>
        {
            opt.AddPolicy(
                EnumPolicyMaster.ADMINISTRADOR, policy =>
                   policy.RequireAssertion(
                        context => context.User.HasClaim(
                            c => c.Type == EnumProcesosBase.ADMINISTRACION && c.Value == EnumRolesBase.ADMINISTRADOR
                        )
                   )
            );

            opt.AddPolicy(
                EnumPolicyMaster.ADMINISTRADOR_ENTIDAD, policy =>
                    policy.RequireAssertion(
                        context => context.User.HasClaim(
                            c => c.Type == EnumProcesosBase.ADMINISTRACION
                                && (c.Value == EnumRolesBase.ADMINISTRADOR
                                    || c.Value == EnumRolesBase.ADMINISTRADOR_ENTIDAD)
                        )
                    )
                );

            opt.AddPolicy(
                EnumPolicyMaster.ADMINISTRADOR_ENTIDAD_UNIDAD, policy =>
                    policy.RequireAssertion(
                        context => context.User.HasClaim(
                            c => c.Type == EnumProcesosBase.ADMINISTRACION
                                && (c.Value == EnumRolesBase.ADMINISTRADOR
                                    || c.Value == EnumRolesBase.ADMINISTRADOR_ENTIDAD
                                    || c.Value == EnumRolesBase.ADMINISTRADOR_UNIDAD)
                        )
                    )
                );

            opt.AddPolicy(
                EnumPolicyMaster.ADMINISTRADOR_ENTIDAD_UNIDAD, policy =>
                    policy.RequireAssertion(
                        context => context.User.HasClaim(
                            c => c.Type == EnumProcesosBase.ADMINISTRACION
                                && (c.Value == EnumRolesBase.ADMINISTRADOR
                                    || c.Value == EnumRolesBase.ADMINISTRADOR_ENTIDAD
                                    || c.Value == EnumRolesBase.ADMINISTRADOR_UNIDAD
                                    || c.Value == EnumRolesBase.USUARIO)
                        )
                    )
                );

            opt.AddPolicy(
                EnumPolicyMaster.VALIDA_ASIGNACION_ROLES, policy =>
                    policy.RequireAssertion(
                        context => context.User.HasClaim(
                            c => c.Type == EnumProcesosBase.ADMINISTRACION
                                && (c.Value == EnumRolesBase.ADMINISTRADOR
                                    || c.Value == EnumRolesBase.VALIDA_ASIGNACION_ROLES)
                        )
                    )
                );

            opt.AddPolicy(
                EnumPolicyMaster.VALIDA_ENRROLAMIENTO, policy =>
                    policy.RequireAssertion(
                        context => context.User.HasClaim(
                            c => c.Type == EnumProcesosBase.ADMINISTRACION
                                && (c.Value == EnumRolesBase.ADMINISTRADOR
                                    || c.Value == EnumRolesBase.VALIDA_ENRROLAMIENTO)
                        )
                    )
                );

            opt.AddPolicy(
                EnumPolicyMaster.USUARIO_LOGUEADO, policy =>
                    policy.RequireAuthenticatedUser()
                );
        });

        return services;
    }
}
