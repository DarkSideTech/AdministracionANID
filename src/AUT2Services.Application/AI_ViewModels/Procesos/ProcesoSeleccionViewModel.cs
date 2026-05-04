using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AUT2Services.Application.ViewModels.Procesos;

public class ProcesoSeleccionViewModel
{
    [Key]
    [DisplayName("Id")]
    public Guid? Id { get; set; }

    [DisplayName("Codigo")]
    public string? Codigo { get; set; }

    [DisplayName("Nombre")]
    public string? Nombre { get; set; }
}
