// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2026-03-02 21:15:49.905
// -------------------------------------------------
using System.ComponentModel;

using AUT2Services.Application.ViewModels;
namespace AUT2Services.Application.ViewModels.Proveedores;

public class ModificarProveedorViewModel
{
    [DisplayName("Id")] 
    public Guid? Id { get; set; } 

    [DisplayName("Nombre")] 
    public string? Nombre { get; set; } 

    [DisplayName("Descripcion")] 
    public string? Descripcion { get; set; } 

    [DisplayName("APIDeAutenticacion")] 
    public string? APIDeAutenticacion { get; set; } 

}

