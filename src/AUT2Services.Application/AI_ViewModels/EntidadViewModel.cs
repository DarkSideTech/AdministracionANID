// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2025-12-03 21:03:27.128
// -------------------------------------------------
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AUT2Services.Application.ViewModels;

public class EntidadViewModel
{
    [Key] 
    [DisplayName("Id")] 
    public Guid? Id { get; set; } 
     
    [DisplayName("Id_UnidadOrganizacional")] 
    public Guid? Id_UnidadOrganizacional { get; set; } 
     
    [DisplayName("Id_Usuario")] 
    public Guid? Id_Usuario { get; set; } 
     
    [DisplayName("TipoDeEntidad")] 
    public string? TipoDeEntidad { get; set; } 
     
    [DisplayName("CorreoElectronico")] 
    public string? CorreoElectronico { get; set; } 
     
    [DisplayName("FechaInicioAutorizacion")] 
    public DateTimeOffset? FechaInicioAutorizacion { get; set; } 
     
    [DisplayName("FechaTerminoAutorizacion")] 
    public DateTimeOffset? FechaTerminoAutorizacion { get; set; } 
     
    [DisplayName("FechaCreacion")] 
    public DateTimeOffset? FechaCreacion { get; set; } 
     
    [DisplayName("Principal")] 
    public bool? Principal { get; set; } 
     
    [DisplayName("EntidadBase")] 
    public bool? EntidadBase { get; set; } 
}

