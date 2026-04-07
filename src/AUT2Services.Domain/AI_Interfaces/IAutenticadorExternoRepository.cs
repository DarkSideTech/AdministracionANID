// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2026-04-05 15:14:10.075
// -------------------------------------------------
 
using AUT2Services.Domain.Core.Data;
using AUT2Services.Domain.Entities;

namespace AUT2Services.Domain.Interfaces;

public interface IAutenticadorExternoRepository : IRepository<AutenticadorExterno>
{
    void Crear(AutenticadorExterno data);
    void Modificar(AutenticadorExterno data);
    void Eliminar(AutenticadorExterno data);
    Task<AutenticadorExterno> BuscarPor_Id(
        Guid id 
        );
    Task<IEnumerable<AutenticadorExterno>> BuscarPor_Id_Proveedor(
        Guid id_Proveedor 
        );
    Task<IEnumerable<AutenticadorExterno>> BuscarPor_Id_Usuario(
        Guid id_Usuario 
        );
    Task<AutenticadorExterno> BuscarPor_Id_Usuario_ValidadorPrimario(
        Guid id_Usuario 
        );
    Task<AutenticadorExterno> BuscarPor_Id_Proveedor_Id_Usuario(
        Guid id_Proveedor, 
        Guid id_Usuario 
        );
}

