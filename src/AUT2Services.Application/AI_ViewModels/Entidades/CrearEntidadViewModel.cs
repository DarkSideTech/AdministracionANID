// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2025-12-03 21:03:27.128
// -------------------------------------------------
using System.ComponentModel;

using AUT2Services.Application.ViewModels;
namespace AUT2Services.Application.ViewModels.Entidades;

public class CrearEntidadViewModel
{
    [DisplayName("Id_UnidadOrganizacional")] 
    public Guid? Id_UnidadOrganizacional { get; set; } 

    [DisplayName("Id_Usuario")] 
    public Guid? Id_Usuario { get; set; } 

    [DisplayName("TipoDeEntidad")] 
    public string? TipoDeEntidad { get; set; } 

    [DisplayName("CorreoElectronico")] 
    public string? CorreoElectronico { get; set; } 

}

