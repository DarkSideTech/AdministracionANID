// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2026-03-02 21:15:49.910
// -------------------------------------------------
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AUT2Services.Application.ViewModels;

public class OrganizacionViewModel
{
    [Key] 
    [DisplayName("Id")] 
    public Guid? Id { get; set; } 
     
    [DisplayName("IdOrganizacion")] 
    public string? IdOrganizacion { get; set; } 
     
    [DisplayName("Codigo")] 
    public string? Codigo { get; set; } 
     
    [DisplayName("Nombre")] 
    public string? Nombre { get; set; } 
     
    [DisplayName("Descripcion")] 
    public string? Descripcion { get; set; } 
     
    [DisplayName("OrganizacionBase")] 
    public bool? OrganizacionBase { get; set; } 
     
    [DisplayName("Activo")] 
    public bool? Activo { get; set; } 
}

