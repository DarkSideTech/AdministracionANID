// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2026-03-02 21:15:49.907
// -------------------------------------------------
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AUT2Services.Application.ViewModels;

public class ValidacionEnrrolamientoViewModel
{
    [Key] 
    [DisplayName("Id")] 
    public Guid? Id { get; set; } 
     
    [DisplayName("IdValidado_Usuario")] 
    public Guid? IdValidado_Usuario { get; set; } 
     
    [DisplayName("IdValidaEnrrolamiento_Usuario")] 
    public Guid? IdValidaEnrrolamiento_Usuario { get; set; } 
     
    [DisplayName("EnrrolamientoAceptado")] 
    public bool? EnrrolamientoAceptado { get; set; } 
     
    [DisplayName("FechaValidacion")] 
    public DateTimeOffset? FechaValidacion { get; set; } 
     
    [DisplayName("FechaRegistro")] 
    public DateTimeOffset? FechaRegistro { get; set; } 
     
    [DisplayName("Activo")] 
    public bool? Activo { get; set; } 
}

