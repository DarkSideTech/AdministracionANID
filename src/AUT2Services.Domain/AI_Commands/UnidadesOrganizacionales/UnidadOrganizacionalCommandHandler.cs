// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2026-04-05 15:14:10.131
// -------------------------------------------------
using AUT2Services.Domain.Core.Commands;
using AUT2Services.Domain.Interfaces;

namespace AUT2Services.Domain.Commands.UnidadesOrganizacionales.Handlers;

public partial class UnidadOrganizacionalCommandHandler : CommandHandler
{
        private readonly IUnidadOrganizacionalRepository _unidadOrganizacionalRepository;
        public UnidadOrganizacionalCommandHandler(
            IUnidadOrganizacionalRepository unidadOrganizacionalRepository
            )
    {
            _unidadOrganizacionalRepository = unidadOrganizacionalRepository ?? throw new ArgumentNullException(nameof(unidadOrganizacionalRepository));
        }
}

