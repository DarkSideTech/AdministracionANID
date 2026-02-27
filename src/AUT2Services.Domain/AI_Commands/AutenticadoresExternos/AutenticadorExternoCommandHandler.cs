// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2026-01-21 15:35:09.249
// -------------------------------------------------
using AUT2Services.Domain.Core.Commands;
using AUT2Services.Domain.Interfaces;

namespace AUT2Services.Domain.Commands.AutenticadoresExternos.Handlers;

public partial class AutenticadorExternoCommandHandler : CommandHandler
{
        private readonly IAutenticadorExternoRepository _autenticadorExternoRepository;
        public AutenticadorExternoCommandHandler(
            IAutenticadorExternoRepository autenticadorExternoRepository
            )
    {
            _autenticadorExternoRepository = autenticadorExternoRepository ?? throw new ArgumentNullException(nameof(autenticadorExternoRepository));
        }
}

