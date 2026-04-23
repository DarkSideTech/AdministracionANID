// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2026-04-05 15:14:10.122
// -------------------------------------------------
using AUT2Services.Domain.Core.Auditing;
using AUT2Services.Domain.Core.Commands;
using AUT2Services.Domain.Interfaces;

namespace AUT2Services.Domain.Commands.AutenticadoresExternos.Handlers;

public partial class AutenticadorExternoCommandHandler : CommandHandler
{
    private readonly IAutenticadorExternoRepository _autenticadorExternoRepository;

    public AutenticadorExternoCommandHandler(
        IAutenticadorExternoRepository autenticadorExternoRepository,
        IAuditBuffer auditBuffer)
    {
        _autenticadorExternoRepository = autenticadorExternoRepository ?? throw new ArgumentNullException(nameof(autenticadorExternoRepository));
        SetAuditBuffer(auditBuffer);
    }
}

