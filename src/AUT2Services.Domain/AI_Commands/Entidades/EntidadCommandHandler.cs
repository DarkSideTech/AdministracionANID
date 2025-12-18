// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2025-12-03 21:03:27.107
// -------------------------------------------------
using AUT2Services.Domain.Core.Commands;
using AUT2Services.Domain.Interfaces;

namespace AUT2Services.Domain.Commands.Entidades.Handlers;

public partial class EntidadCommandHandler : CommandHandler
{
        private readonly IEntidadRepository _entidadRepository;
        public EntidadCommandHandler(
            IEntidadRepository entidadRepository
            )
    {
            _entidadRepository = entidadRepository ?? throw new ArgumentNullException(nameof(entidadRepository));
        }
}

