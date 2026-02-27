// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2026-01-21 15:35:09.291
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

