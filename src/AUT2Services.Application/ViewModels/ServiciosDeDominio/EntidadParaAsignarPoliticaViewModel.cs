using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AUT2Services.Application.ViewModels.ServiciosDeDominio;

public class EntidadParaAsignarPoliticaViewModel
{
    [Key]
    [DisplayName("IdEntidad")]
    public Guid? IdEntidad { get; set; }

    [DisplayName("IdUsuario")]
    public Guid? IdUsuario { get; set; }

    [DisplayName("NombreUsuario")]
    public string? NombreUsuario { get; set; }

    [DisplayName("IdUnidadOrganizacional")]
    public Guid? IdUnidadOrganizacional { get; set; }

    [DisplayName("CodigoUnidadOrganizacional")]
    public string? CodigoUnidadOrganizacional { get; set; }

    [DisplayName("NombreUnidadOrganizacional")]
    public string? NombreUnidadOrganizacional { get; set; }

    [DisplayName("TipoDeEntidad")]
    public string? TipoDeEntidad { get; set; }

    [DisplayName("CorreoElectronico")]
    public string? CorreoElectronico { get; set; }

    [DisplayName("Principal")]
    public bool? Principal { get; set; }
}
