// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2026-04-05 15:14:10.141
// -------------------------------------------------
using AUT2Services.Domain.Core.Auditing;
using AUT2Services.Domain.Core.Commands;
using AUT2Services.Domain.Interfaces;

namespace AUT2Services.Domain.Commands.Organizaciones.Handlers;

public partial class OrganizacionCommandHandler : CommandHandler
{
    private readonly IOrganizacionRepository _organizacionRepository;

    public OrganizacionCommandHandler(
        IOrganizacionRepository organizacionRepository,
        IAuditBuffer auditBuffer)
    {
        _organizacionRepository = organizacionRepository ?? throw new ArgumentNullException(nameof(organizacionRepository));
        SetAuditBuffer(auditBuffer);
    }
}

