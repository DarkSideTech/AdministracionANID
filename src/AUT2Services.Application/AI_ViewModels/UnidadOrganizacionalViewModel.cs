// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2026-01-21 15:35:09.293
// -------------------------------------------------
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AUT2Services.Application.ViewModels;

public class UnidadOrganizacionalViewModel
{
    [Key] 
    [DisplayName("Id")] 
    public Guid? Id { get; set; } 
     
    [DisplayName("Id_Organizacion")] 
    public Guid? Id_Organizacion { get; set; } 
     
    [DisplayName("Codigo")] 
    public string? Codigo { get; set; } 
     
    [DisplayName("Nombre")] 
    public string? Nombre { get; set; } 
     
    [DisplayName("Descripcion")] 
    public string? Descripcion { get; set; } 
     
    [DisplayName("UnidadOrganizacionalBase")] 
    public bool? UnidadOrganizacionalBase { get; set; } 
     
    [DisplayName("Activo")] 
    public bool? Activo { get; set; } 
}

