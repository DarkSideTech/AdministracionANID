// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2025-12-03 21:03:27.146
// -------------------------------------------------
using AUT2Services.Application.Interfaces;
using AUT2Services.Application.Services;
using AUT2Services.Domain.Core.Commands;
using AUT2Services.Domain.Core.Mediator;
using AUT2Services.Domain.Interfaces;
using AUT2Services.Infra.Cross.Bus;
using AUT2Services.Infra.Data.Context;
using AUT2Services.Infra.Data.Repositories;
using AUT2Services.Domain.Commands.Proveedores.Commands;
using AUT2Services.Domain.Commands.Proveedores.Handlers;
using AUT2Services.Domain.Events.Proveedores;
using AUT2Services.Domain.Events.Proveedores.Events;
using AUT2Services.Domain.Commands.AutenticadoresExternos.Commands;
using AUT2Services.Domain.Commands.AutenticadoresExternos.Handlers;
using AUT2Services.Domain.Events.AutenticadoresExternos;
using AUT2Services.Domain.Events.AutenticadoresExternos.Events;
using AUT2Services.Domain.Commands.ValidacionEnrrolamientos.Commands;
using AUT2Services.Domain.Commands.ValidacionEnrrolamientos.Handlers;
using AUT2Services.Domain.Events.ValidacionEnrrolamientos;
using AUT2Services.Domain.Events.ValidacionEnrrolamientos.Events;
using AUT2Services.Domain.Commands.UnidadesOrganizacionales.Commands;
using AUT2Services.Domain.Commands.UnidadesOrganizacionales.Handlers;
using AUT2Services.Domain.Events.UnidadesOrganizacionales;
using AUT2Services.Domain.Events.UnidadesOrganizacionales.Events;
using AUT2Services.Domain.Commands.Entidades.Commands;
using AUT2Services.Domain.Commands.Entidades.Handlers;
using AUT2Services.Domain.Events.Entidades;
using AUT2Services.Domain.Events.Entidades.Events;
using AUT2Services.Domain.Commands.Organizaciones.Commands;
using AUT2Services.Domain.Commands.Organizaciones.Handlers;
using AUT2Services.Domain.Events.Organizaciones;
using AUT2Services.Domain.Events.Organizaciones.Events;
using AUT2Services.Domain.Commands.PoliticasAsignadas.Commands;
using AUT2Services.Domain.Commands.PoliticasAsignadas.Handlers;
using AUT2Services.Domain.Events.PoliticasAsignadas;
using AUT2Services.Domain.Events.PoliticasAsignadas.Events;
using AUT2Services.Domain.Commands.Procesos.Commands;
using AUT2Services.Domain.Commands.Procesos.Handlers;
using AUT2Services.Domain.Events.Procesos;
using AUT2Services.Domain.Events.Procesos.Events;
using AUT2Services.Domain.Events.ServiciosDeDominios;
using AUT2Services.Domain.Events.ServiciosDeDominios.Events;
using Microsoft.Extensions.DependencyInjection;

namespace AUT2Services.Infra.Cross.IoC;

public class AI_NativeInjectorBootStrapper
{
    public static void RegisterServices(IServiceCollection services)
    {
        #region Domain Bus (Mediator)
        services.AddScoped<IMediator, Mediator>();
        services.AddScoped<IMediatorHandler, MemoryBus>();
        #endregion

        #region Application
        services.AddScoped<IProveedorServiceApp, ProveedorServiceApp>();
        services.AddScoped<IAutenticadorExternoServiceApp, AutenticadorExternoServiceApp>();
        services.AddScoped<IValidacionEnrrolamientoServiceApp, ValidacionEnrrolamientoServiceApp>();
        services.AddScoped<IUnidadOrganizacionalServiceApp, UnidadOrganizacionalServiceApp>();
        services.AddScoped<IEntidadServiceApp, EntidadServiceApp>();
        services.AddScoped<IOrganizacionServiceApp, OrganizacionServiceApp>();
        services.AddScoped<IPoliticaAsignadaServiceApp, PoliticaAsignadaServiceApp>();
        services.AddScoped<IProcesoServiceApp, ProcesoServiceApp>();
        #endregion

        #region Domain - Events
        //Proveedor
        services.AddScoped<INotificationHandler<ProveedorEventCreado>, ProveedorEventHandler>(); 
        services.AddScoped<INotificationHandler<ProveedorEventModificado>, ProveedorEventHandler>(); 
        services.AddScoped<INotificationHandler<ProveedorEventEliminado>, ProveedorEventHandler>(); 
        services.AddScoped<INotificationHandler<ProveedorEventEliminadoPor_Codigo>, ProveedorEventHandler>(); 
        services.AddScoped<INotificationHandler<ProveedorEventActivado>, ProveedorEventHandler>(); 
        services.AddScoped<INotificationHandler<ProveedorEventDesactivado>, ProveedorEventHandler>(); 
 
        //AutenticadorExterno
        services.AddScoped<INotificationHandler<AutenticadorExternoEventCreado>, AutenticadorExternoEventHandler>(); 
        services.AddScoped<INotificationHandler<AutenticadorExternoEventModificado>, AutenticadorExternoEventHandler>(); 
        services.AddScoped<INotificationHandler<AutenticadorExternoEventEliminado>, AutenticadorExternoEventHandler>(); 
        services.AddScoped<INotificationHandler<AutenticadorExternoEventValidadorPrimarioMarcado>, AutenticadorExternoEventHandler>(); 
        services.AddScoped<INotificationHandler<AutenticadorExternoEventActivado>, AutenticadorExternoEventHandler>(); 
        services.AddScoped<INotificationHandler<AutenticadorExternoEventDesactivado>, AutenticadorExternoEventHandler>(); 
 
        //ValidacionEnrrolamiento
        services.AddScoped<INotificationHandler<ValidacionEnrrolamientoEventCreado>, ValidacionEnrrolamientoEventHandler>(); 
        services.AddScoped<INotificationHandler<ValidacionEnrrolamientoEventEliminado>, ValidacionEnrrolamientoEventHandler>(); 
        services.AddScoped<INotificationHandler<ValidacionEnrrolamientoEventActivado>, ValidacionEnrrolamientoEventHandler>(); 
        services.AddScoped<INotificationHandler<ValidacionEnrrolamientoEventDesactivado>, ValidacionEnrrolamientoEventHandler>(); 
 
        //UnidadOrganizacional
        services.AddScoped<INotificationHandler<UnidadOrganizacionalEventCreado>, UnidadOrganizacionalEventHandler>(); 
        services.AddScoped<INotificationHandler<UnidadOrganizacionalEventModificado>, UnidadOrganizacionalEventHandler>(); 
        services.AddScoped<INotificationHandler<UnidadOrganizacionalEventEliminado>, UnidadOrganizacionalEventHandler>(); 
        services.AddScoped<INotificationHandler<UnidadOrganizacionalEventEliminadoPor_Codigo_Id_Organizacion>, UnidadOrganizacionalEventHandler>(); 
        services.AddScoped<INotificationHandler<UnidadOrganizacionalEventActivado>, UnidadOrganizacionalEventHandler>(); 
        services.AddScoped<INotificationHandler<UnidadOrganizacionalEventDesactivado>, UnidadOrganizacionalEventHandler>(); 
 
        //Entidad
        services.AddScoped<INotificationHandler<EntidadEventCreado>, EntidadEventHandler>(); 
        services.AddScoped<INotificationHandler<EntidadEventModificado>, EntidadEventHandler>(); 
        services.AddScoped<INotificationHandler<EntidadEventEliminado>, EntidadEventHandler>(); 
        services.AddScoped<INotificationHandler<EntidadEventAutorizacionFinalizada>, EntidadEventHandler>(); 
        services.AddScoped<INotificationHandler<EntidadEventEntidadCambiadaAPrincipal>, EntidadEventHandler>(); 
        services.AddScoped<INotificationHandler<EntidadEventEntidadCambiadaANoPrincipal>, EntidadEventHandler>(); 
 
        //Organizacion
        services.AddScoped<INotificationHandler<OrganizacionEventCreado>, OrganizacionEventHandler>(); 
        services.AddScoped<INotificationHandler<OrganizacionEventModificado>, OrganizacionEventHandler>(); 
        services.AddScoped<INotificationHandler<OrganizacionEventEliminado>, OrganizacionEventHandler>(); 
        services.AddScoped<INotificationHandler<OrganizacionEventEliminadoPor_Codigo>, OrganizacionEventHandler>(); 
        services.AddScoped<INotificationHandler<OrganizacionEventActivado>, OrganizacionEventHandler>(); 
        services.AddScoped<INotificationHandler<OrganizacionEventDesactivado>, OrganizacionEventHandler>(); 
 
        //PoliticaAsignada
        services.AddScoped<INotificationHandler<PoliticaAsignadaEventCreado>, PoliticaAsignadaEventHandler>(); 
        services.AddScoped<INotificationHandler<PoliticaAsignadaEventCreadoAsignadoNuevaEntidadPersona>, PoliticaAsignadaEventHandler>(); 
        services.AddScoped<INotificationHandler<PoliticaAsignadaEventEliminado>, PoliticaAsignadaEventHandler>(); 
        services.AddScoped<INotificationHandler<PoliticaAsignadaEventAsignacionFinalizada>, PoliticaAsignadaEventHandler>(); 
        services.AddScoped<INotificationHandler<PoliticaAsignadaEventAsignacionDeRolValidada>, PoliticaAsignadaEventHandler>(); 
 
        //Proceso
        services.AddScoped<INotificationHandler<ProcesoEventCreado>, ProcesoEventHandler>(); 
        services.AddScoped<INotificationHandler<ProcesoEventModificado>, ProcesoEventHandler>(); 
        services.AddScoped<INotificationHandler<ProcesoEventEliminado>, ProcesoEventHandler>(); 
        services.AddScoped<INotificationHandler<ProcesoEventActivada>, ProcesoEventHandler>(); 
        services.AddScoped<INotificationHandler<ProcesoEventDesactivada>, ProcesoEventHandler>(); 
 
        //ServicioDeDominio
        services.AddScoped<INotificationHandler<ServicioDeDominioEventEnrrolamientoValidado>, ServicioDeDominioEventHandler>(); 
        services.AddScoped<INotificationHandler<ServicioDeDominioEventAsignacionDeRolValidada>, ServicioDeDominioEventHandler>(); 
        services.AddScoped<INotificationHandler<ServicioDeDominioEventEntidadNuevaCreada>, ServicioDeDominioEventHandler>(); 
        services.AddScoped<INotificationHandler<ServicioDeDominioEventEntidadMarcadaComoPrincipal>, ServicioDeDominioEventHandler>(); 
 
        #endregion

        #region Domain - Commands
        //Proveedor
        services.AddScoped<IRequestHandler<CrearProveedorCommand, CommandResponse>, ProveedorCommandHandler>();
        services.AddScoped<IRequestHandler<ModificarProveedorCommand, CommandResponse>, ProveedorCommandHandler>();
        services.AddScoped<IRequestHandler<EliminarProveedorCommand, CommandResponse>, ProveedorCommandHandler>();
        services.AddScoped<IRequestHandler<EliminarPor_CodigoProveedorCommand, CommandResponse>, ProveedorCommandHandler>();
        services.AddScoped<IRequestHandler<ActivarProveedorCommand, CommandResponse>, ProveedorCommandHandler>();
        services.AddScoped<IRequestHandler<DesactivarProveedorCommand, CommandResponse>, ProveedorCommandHandler>();
 
        //AutenticadorExterno
        services.AddScoped<IRequestHandler<CrearAutenticadorExternoCommand, CommandResponse>, AutenticadorExternoCommandHandler>();
        services.AddScoped<IRequestHandler<ModificarAutenticadorExternoCommand, CommandResponse>, AutenticadorExternoCommandHandler>();
        services.AddScoped<IRequestHandler<EliminarAutenticadorExternoCommand, CommandResponse>, AutenticadorExternoCommandHandler>();
        services.AddScoped<IRequestHandler<MarcarComoValidadorPrimarioAutenticadorExternoCommand, CommandResponse>, AutenticadorExternoCommandHandler>();
        services.AddScoped<IRequestHandler<ActivarAutenticadorExternoCommand, CommandResponse>, AutenticadorExternoCommandHandler>();
        services.AddScoped<IRequestHandler<DesactivarAutenticadorExternoCommand, CommandResponse>, AutenticadorExternoCommandHandler>();
 
        //ValidacionEnrrolamiento
        services.AddScoped<IRequestHandler<CrearValidacionEnrrolamientoCommand, CommandResponse>, ValidacionEnrrolamientoCommandHandler>();
        services.AddScoped<IRequestHandler<EliminarValidacionEnrrolamientoCommand, CommandResponse>, ValidacionEnrrolamientoCommandHandler>();
        services.AddScoped<IRequestHandler<ActivarValidacionEnrrolamientoCommand, CommandResponse>, ValidacionEnrrolamientoCommandHandler>();
        services.AddScoped<IRequestHandler<DesactivarValidacionEnrrolamientoCommand, CommandResponse>, ValidacionEnrrolamientoCommandHandler>();
 
        //UnidadOrganizacional
        services.AddScoped<IRequestHandler<CrearUnidadOrganizacionalCommand, CommandResponse>, UnidadOrganizacionalCommandHandler>();
        services.AddScoped<IRequestHandler<ModificarUnidadOrganizacionalCommand, CommandResponse>, UnidadOrganizacionalCommandHandler>();
        services.AddScoped<IRequestHandler<EliminarUnidadOrganizacionalCommand, CommandResponse>, UnidadOrganizacionalCommandHandler>();
        services.AddScoped<IRequestHandler<EliminarPor_Codigo_Id_OrganizacionUnidadOrganizacionalCommand, CommandResponse>, UnidadOrganizacionalCommandHandler>();
        services.AddScoped<IRequestHandler<ActivarUnidadOrganizacionalCommand, CommandResponse>, UnidadOrganizacionalCommandHandler>();
        services.AddScoped<IRequestHandler<DesactivarUnidadOrganizacionalCommand, CommandResponse>, UnidadOrganizacionalCommandHandler>();
 
        //Entidad
        services.AddScoped<IRequestHandler<CrearEntidadCommand, CommandResponse>, EntidadCommandHandler>();
        services.AddScoped<IRequestHandler<ModificarEntidadCommand, CommandResponse>, EntidadCommandHandler>();
        services.AddScoped<IRequestHandler<EliminarEntidadCommand, CommandResponse>, EntidadCommandHandler>();
        services.AddScoped<IRequestHandler<FinalizaAutorizacionEntidadCommand, CommandResponse>, EntidadCommandHandler>();
        services.AddScoped<IRequestHandler<CambiaEntidadAPrincipalEntidadCommand, CommandResponse>, EntidadCommandHandler>();
        services.AddScoped<IRequestHandler<CambiaEntidadANoPrincipalEntidadCommand, CommandResponse>, EntidadCommandHandler>();
 
        //Organizacion
        services.AddScoped<IRequestHandler<CrearOrganizacionCommand, CommandResponse>, OrganizacionCommandHandler>();
        services.AddScoped<IRequestHandler<ModificarOrganizacionCommand, CommandResponse>, OrganizacionCommandHandler>();
        services.AddScoped<IRequestHandler<EliminarOrganizacionCommand, CommandResponse>, OrganizacionCommandHandler>();
        services.AddScoped<IRequestHandler<EliminarPor_CodigoOrganizacionCommand, CommandResponse>, OrganizacionCommandHandler>();
        services.AddScoped<IRequestHandler<ActivarOrganizacionCommand, CommandResponse>, OrganizacionCommandHandler>();
        services.AddScoped<IRequestHandler<DesactivarOrganizacionCommand, CommandResponse>, OrganizacionCommandHandler>();
 
        //PoliticaAsignada
        services.AddScoped<IRequestHandler<CrearPoliticaAsignadaCommand, CommandResponse>, PoliticaAsignadaCommandHandler>();
        services.AddScoped<IRequestHandler<CrearAsignarNuevaEntidadPersonaPoliticaAsignadaCommand, CommandResponse>, PoliticaAsignadaCommandHandler>();
        services.AddScoped<IRequestHandler<EliminarPoliticaAsignadaCommand, CommandResponse>, PoliticaAsignadaCommandHandler>();
        services.AddScoped<IRequestHandler<FinalizaAsignacionPoliticaAsignadaCommand, CommandResponse>, PoliticaAsignadaCommandHandler>();
        services.AddScoped<IRequestHandler<ValidaAsignacionDeRolPoliticaAsignadaCommand, CommandResponse>, PoliticaAsignadaCommandHandler>();
 
        //Proceso
        services.AddScoped<IRequestHandler<CrearProcesoCommand, CommandResponse>, ProcesoCommandHandler>();
        services.AddScoped<IRequestHandler<ModificarProcesoCommand, CommandResponse>, ProcesoCommandHandler>();
        services.AddScoped<IRequestHandler<EliminarProcesoCommand, CommandResponse>, ProcesoCommandHandler>();
        services.AddScoped<IRequestHandler<ActivarProcesoCommand, CommandResponse>, ProcesoCommandHandler>();
        services.AddScoped<IRequestHandler<DesactivarProcesoCommand, CommandResponse>, ProcesoCommandHandler>();
 
        //ServicioDeDominio
 
        #endregion

        #region Infraestructure - Data
        services.AddScoped<IProveedorRepository, ProveedorRepository>();
        services.AddScoped<IAutenticadorExternoRepository, AutenticadorExternoRepository>();
        services.AddScoped<IValidacionEnrrolamientoRepository, ValidacionEnrrolamientoRepository>();
        services.AddScoped<IUnidadOrganizacionalRepository, UnidadOrganizacionalRepository>();
        services.AddScoped<IEntidadRepository, EntidadRepository>();
        services.AddScoped<IOrganizacionRepository, OrganizacionRepository>();
        services.AddScoped<IPoliticaAsignadaRepository, PoliticaAsignadaRepository>();
        services.AddScoped<IProcesoRepository, ProcesoRepository>();

        services.AddScoped<AUT2ServicesContext>();
        services.AddDbContext<AUT2ServicesContext>();
        #endregion
    } 
}

