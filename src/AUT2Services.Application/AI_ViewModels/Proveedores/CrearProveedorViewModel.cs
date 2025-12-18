// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2025-12-03 21:03:27.124
// -------------------------------------------------
using System.ComponentModel;

using AUT2Services.Application.ViewModels;
namespace AUT2Services.Application.ViewModels.Proveedores;

public class CrearProveedorViewModel
{
    [DisplayName("Codigo")] 
    public string? Codigo { get; set; } 

    [DisplayName("Nombre")] 
    public string? Nombre { get; set; } 

    [DisplayName("Descripcion")] 
    public string? Descripcion { get; set; } 

    [DisplayName("APIDeAutenticacion")] 
    public string? APIDeAutenticacion { get; set; } 

}

