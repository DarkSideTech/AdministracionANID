// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2025-12-03 21:03:27.132
// -------------------------------------------------
using System.ComponentModel;

using AUT2Services.Application.ViewModels;
namespace AUT2Services.Application.ViewModels.Procesos;

public class ModificarProcesoViewModel
{
    [DisplayName("Id")] 
    public Guid? Id { get; set; } 

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

    [DisplayName("MaximaAsignacionDeRoles")] 
    public int? MaximaAsignacionDeRoles { get; set; } 

}

