// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2026-04-05 14:21:10.419
// -------------------------------------------------
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AUT2Services.Application.ViewModels;

public class ProcesoViewModel
{
    [Key] 
    [DisplayName("Id")] 
    public Guid? Id { get; set; } 
     
    [DisplayName("IdMacro_Proceso")] 
    public Guid? IdMacro_Proceso { get; set; } 
     
    [DisplayName("Codigo")] 
    public string? Codigo { get; set; } 
     
    [DisplayName("Nombre")] 
    public string? Nombre { get; set; } 
     
    [DisplayName("Descripcion")] 
    public string? Descripcion { get; set; } 
     
    [DisplayName("Contexto")] 
    public string? Contexto { get; set; } 
     
    [DisplayName("NivelDeProceso")] 
    public string? NivelDeProceso { get; set; } 
     
    [DisplayName("Url")] 
    public string? Url { get; set; } 
     
    [DisplayName("Token")] 
    public string? Token { get; set; } 
     
    [DisplayName("ComoDesplegarUrlDeProceso")] 
    public string? ComoDesplegarUrlDeProceso { get; set; } 
     
    [DisplayName("ProcesoBase")] 
    public bool? ProcesoBase { get; set; } 
     
    [DisplayName("MaximaAsignacionDeRoles")] 
    public int? MaximaAsignacionDeRoles { get; set; } 
     
    [DisplayName("Activo")] 
    public bool? Activo { get; set; } 
}

