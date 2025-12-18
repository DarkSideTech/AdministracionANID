// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2025-12-03 21:03:27.111
// -------------------------------------------------
using AUT2Services.Domain.Core.Commands;
using AUT2Services.Domain.Interfaces;

namespace AUT2Services.Domain.Commands.Organizaciones.Handlers;

public partial class OrganizacionCommandHandler : CommandHandler
{
        private readonly IOrganizacionRepository _organizacionRepository;
        public OrganizacionCommandHandler(
            IOrganizacionRepository organizacionRepository
            )
    {
            _organizacionRepository = organizacionRepository ?? throw new ArgumentNullException(nameof(organizacionRepository));
        }
}

