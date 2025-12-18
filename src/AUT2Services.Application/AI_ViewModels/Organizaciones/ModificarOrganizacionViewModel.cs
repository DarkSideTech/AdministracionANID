// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2025-12-03 21:03:27.130
// -------------------------------------------------
using System.ComponentModel;

using AUT2Services.Application.ViewModels;
namespace AUT2Services.Application.ViewModels.Organizaciones;

public class ModificarOrganizacionViewModel
{
    [DisplayName("Id")] 
    public Guid? Id { get; set; } 

    [DisplayName("IdOrganizacion")] 
    public string? IdOrganizacion { get; set; } 

    [DisplayName("Nombre")] 
    public string? Nombre { get; set; } 

    [DisplayName("Descripcion")] 
    public string? Descripcion { get; set; } 

}

