// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2025-12-03 21:03:27.125
// -------------------------------------------------
using System.ComponentModel;

using AUT2Services.Application.ViewModels;
namespace AUT2Services.Application.ViewModels.AutenticadoresExternos;

public class CrearAutenticadorExternoViewModel
{
    [DisplayName("Id_Proveedor")] 
    public Guid? Id_Proveedor { get; set; } 

    [DisplayName("Id_Usuario")] 
    public Guid? Id_Usuario { get; set; } 

    [DisplayName("NombreUsuario")] 
    public string? NombreUsuario { get; set; } 

    [DisplayName("ClaveDeAcceso")] 
    public string? ClaveDeAcceso { get; set; } 

    [DisplayName("NombreADesplegar")] 
    public string? NombreADesplegar { get; set; } 

}

