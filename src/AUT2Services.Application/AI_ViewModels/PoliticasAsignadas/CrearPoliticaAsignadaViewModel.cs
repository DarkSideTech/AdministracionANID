// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2026-01-21 15:35:09.297
// -------------------------------------------------
using System.ComponentModel;

using AUT2Services.Application.ViewModels;
namespace AUT2Services.Application.ViewModels.PoliticasAsignadas;

public class CrearPoliticaAsignadaViewModel
{
    [DisplayName("Id_Entidad")] 
    public Guid? Id_Entidad { get; set; } 

    [DisplayName("Id_Rol")] 
    public Guid? Id_Rol { get; set; } 

    [DisplayName("Id_Proceso")] 
    public Guid? Id_Proceso { get; set; } 

    [DisplayName("RolRequiereValidacion")] 
    public bool? RolRequiereValidacion { get; set; } 

}

