using AUT2Services.Application.Interfaces;
using AUT2Services.Domain.Core.Mediator;
using AUT2Services.Domain.Interfaces;
using AUT2Services.Domain.Security.Entities;
using AUT2Services.Infra.Data.Context;
using Microsoft.AspNetCore.Identity;

namespace AUT2Services.Application.Services.ServicioDeDominioHandlers;

public partial class ServicioDeDominioServiceApp : IServicioDeDominioServiceApp
{
    private readonly IMediatorHandler mediator;
    private readonly IServicioDeDominioRepository servicioDeDominioRepository;
    private readonly AUT2ServicesContext context;
    private readonly IUnidadOrganizacionalRepository unidadOrganizacionalRepository;
    private readonly UserManager<Usuario> userManager;
    private readonly RoleManager<Rol> roleManager;
    private readonly IOrganizacionRepository organizacionRepository;
    private readonly IEntidadRepository entidadRepository;
    private readonly IPoliticaAsignadaRepository politicaAsignadaRepository;
    private readonly IProcesoRepository procesoRepository;
    private readonly CancellationToken cancellationToken = default;

    public ServicioDeDominioServiceApp(
                IMediatorHandler mediator,
                UserManager<Usuario> userManager,
                RoleManager<Rol> roleManager,
                AUT2ServicesContext context,
                IServicioDeDominioRepository servicioDeDominioRepository,
                IUnidadOrganizacionalRepository unidadOrganizacionalRepository,
                IOrganizacionRepository organizacionRepository,
                IEntidadRepository entidadRepository,
                IPoliticaAsignadaRepository politicaAsignadaRepository,
                IProcesoRepository procesoRepository
        )
    {
        this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        this.servicioDeDominioRepository = servicioDeDominioRepository;
        this.context = context;
        this.unidadOrganizacionalRepository = unidadOrganizacionalRepository;
        this.userManager = userManager;
        this.roleManager = roleManager;
        this.organizacionRepository = organizacionRepository;
        this.entidadRepository = entidadRepository;
        this.politicaAsignadaRepository = politicaAsignadaRepository;
        this.procesoRepository = procesoRepository;
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }
}

