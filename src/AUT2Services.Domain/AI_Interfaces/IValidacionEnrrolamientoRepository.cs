// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2026-01-21 15:35:09.215
// -------------------------------------------------
 
using AUT2Services.Domain.Core.Data;
using AUT2Services.Domain.Entities;

namespace AUT2Services.Domain.Interfaces;

public interface IValidacionEnrrolamientoRepository : IRepository<ValidacionEnrrolamiento>
{
    void Crear(ValidacionEnrrolamiento data);
    void Modificar(ValidacionEnrrolamiento data);
    void Eliminar(ValidacionEnrrolamiento data);
    Task<ValidacionEnrrolamiento> BuscarPor_Id(
        Guid id 
        );
    Task<ValidacionEnrrolamiento> BuscarPor_IdValidado_Usuario_IdValidaEnrrolamiento_Usuario(
        Guid idValidado_Usuario, 
        Guid idValidaEnrrolamiento_Usuario 
        );
    Task<IEnumerable<ValidacionEnrrolamiento>> BuscarPor_IdValidado_Usuario(
        Guid idValidado_Usuario 
        );
    Task<IEnumerable<ValidacionEnrrolamiento>> BuscarPor_IdValidaEnrrolamiento_Usuario(
        Guid idValidaEnrrolamiento_Usuario 
        );
}

