using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AUT2Services.Application.ViewModels.ServiciosDeDominio;

public class PoliticaAsignadaParaEntidadViewModel
{
    [Key]
    [DisplayName("IdPoliticaAsignada")]
    public Guid? IdPoliticaAsignada { get; set; }

    [DisplayName("IdEntidad")]
    public Guid? IdEntidad { get; set; }

    [DisplayName("IdRol")]
    public Guid? IdRol { get; set; }

    [DisplayName("NombreRol")]
    public string? NombreRol { get; set; }

    [DisplayName("IdProceso")]
    public Guid? IdProceso { get; set; }

    [DisplayName("CodigoProceso")]
    public string? CodigoProceso { get; set; }

    [DisplayName("NombreProceso")]
    public string? NombreProceso { get; set; }

    [DisplayName("RolRequiereValidacion")]
    public bool? RolRequiereValidacion { get; set; }

    [DisplayName("RolAsignadoValidado")]
    public bool? RolAsignadoValidado { get; set; }
}
