// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2025-12-03 21:03:27.115
// -------------------------------------------------
using AUT2Services.Domain.Core.Commands;
using AUT2Services.Domain.Interfaces;

namespace AUT2Services.Domain.Commands.PoliticasAsignadas.Handlers;

public partial class PoliticaAsignadaCommandHandler : CommandHandler
{
        private readonly IPoliticaAsignadaRepository _politicaAsignadaRepository;
        public PoliticaAsignadaCommandHandler(
            IPoliticaAsignadaRepository politicaAsignadaRepository
            )
    {
            _politicaAsignadaRepository = politicaAsignadaRepository ?? throw new ArgumentNullException(nameof(politicaAsignadaRepository));
        }
}

