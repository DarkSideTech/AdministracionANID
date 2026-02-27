// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2026-01-21 15:35:09.264
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

