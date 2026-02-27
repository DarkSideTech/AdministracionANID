// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2026-01-21 15:35:09.296
// -------------------------------------------------
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AUT2Services.Application.ViewModels;

public class PoliticaAsignadaViewModel
{
    [Key] 
    [DisplayName("Id")] 
    public Guid? Id { get; set; } 
     
    [DisplayName("Id_Entidad")] 
    public Guid? Id_Entidad { get; set; } 
     
    [DisplayName("Id_Rol")] 
    public Guid? Id_Rol { get; set; } 
     
    [DisplayName("Id_Proceso")] 
    public Guid? Id_Proceso { get; set; } 
     
    [DisplayName("FechaInicioAsignacion")] 
    public DateTimeOffset? FechaInicioAsignacion { get; set; } 
     
    [DisplayName("FechaTerminoAsignacion")] 
    public DateTimeOffset? FechaTerminoAsignacion { get; set; } 
     
    [DisplayName("FechaCreacion")] 
    public DateTimeOffset? FechaCreacion { get; set; } 
     
    [DisplayName("RolRequiereValidacion")] 
    public bool? RolRequiereValidacion { get; set; } 
     
    [DisplayName("RolAsignadoValidado")] 
    public bool? RolAsignadoValidado { get; set; } 
     
    [DisplayName("PoliticaAsignadaBase")] 
    public bool? PoliticaAsignadaBase { get; set; } 
}

