// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2026-01-21 15:35:09.235
// -------------------------------------------------
using AUT2Services.Domain.Core.Commands;
using AUT2Services.Domain.Interfaces;

namespace AUT2Services.Domain.Commands.Proveedores.Handlers;

public partial class ProveedorCommandHandler : CommandHandler
{
        private readonly IProveedorRepository _proveedorRepository;
        public ProveedorCommandHandler(
            IProveedorRepository proveedorRepository
            )
    {
            _proveedorRepository = proveedorRepository ?? throw new ArgumentNullException(nameof(proveedorRepository));
        }
}

