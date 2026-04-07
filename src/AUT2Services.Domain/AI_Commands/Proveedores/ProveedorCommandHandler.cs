// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2026-04-05 15:14:10.108
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

