// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2026-04-05 14:21:10.411
// -------------------------------------------------
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AUT2Services.Application.ViewModels;

public class AutenticadorExternoViewModel
{
    [Key] 
    [DisplayName("Id")] 
    public Guid? Id { get; set; } 
     
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
     
    [DisplayName("ValidadorPrimario")] 
    public bool? ValidadorPrimario { get; set; } 
     
    [DisplayName("AutenticadorExternoBase")] 
    public bool? AutenticadorExternoBase { get; set; } 
     
    [DisplayName("Activo")] 
    public bool? Activo { get; set; } 
}

