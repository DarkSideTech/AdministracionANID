namespace AUT2Services.Domain.DTOs;

public class ProcesoSeleccionDTO
{
    public Guid Id { get; set; } = Guid.Empty;
    public string Codigo { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
}
