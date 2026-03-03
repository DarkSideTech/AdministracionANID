// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2026-03-02 21:15:49.908
// -------------------------------------------------
using System.ComponentModel;

using AUT2Services.Application.ViewModels;
namespace AUT2Services.Application.ViewModels.UnidadesOrganizacionales;

public class CrearUnidadOrganizacionalViewModel
{
    [DisplayName("Id_Organizacion")] 
    public Guid? Id_Organizacion { get; set; } 

    [DisplayName("Codigo")] 
    public string? Codigo { get; set; } 

    [DisplayName("Nombre")] 
    public string? Nombre { get; set; } 

    [DisplayName("Descripcion")] 
    public string? Descripcion { get; set; } 

}

