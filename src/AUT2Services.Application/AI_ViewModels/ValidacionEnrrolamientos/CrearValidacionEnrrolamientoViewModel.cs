// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2026-03-02 21:15:49.907
// -------------------------------------------------
using System.ComponentModel;

using AUT2Services.Application.ViewModels;
namespace AUT2Services.Application.ViewModels.ValidacionEnrrolamientos;

public class CrearValidacionEnrrolamientoViewModel
{
    [DisplayName("IdValidado_Usuario")] 
    public Guid? IdValidado_Usuario { get; set; } 

    [DisplayName("IdValidaEnrrolamiento_Usuario")] 
    public Guid? IdValidaEnrrolamiento_Usuario { get; set; } 

    [DisplayName("EnrrolamientoAceptado")] 
    public bool? EnrrolamientoAceptado { get; set; } 

    [DisplayName("FechaValidacion")] 
    public DateTimeOffset? FechaValidacion { get; set; } 

}

