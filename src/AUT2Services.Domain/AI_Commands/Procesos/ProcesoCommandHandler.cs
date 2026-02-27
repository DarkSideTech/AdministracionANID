// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2026-01-21 15:35:09.280
// -------------------------------------------------
using AUT2Services.Domain.Core.Commands;
using AUT2Services.Domain.Interfaces;

namespace AUT2Services.Domain.Commands.Procesos.Handlers;

public partial class ProcesoCommandHandler : CommandHandler
{
        private readonly IProcesoRepository _procesoRepository;
        public ProcesoCommandHandler(
            IProcesoRepository procesoRepository
            )
    {
            _procesoRepository = procesoRepository ?? throw new ArgumentNullException(nameof(procesoRepository));
        }
}

