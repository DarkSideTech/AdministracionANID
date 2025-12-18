// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2025-12-03 21:03:27.120
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

