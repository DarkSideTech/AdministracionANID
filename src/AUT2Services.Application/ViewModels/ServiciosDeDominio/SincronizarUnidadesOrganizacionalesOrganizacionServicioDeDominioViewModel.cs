namespace AUT2Services.Application.ViewModels.ServiciosDeDominio;

public sealed class SincronizarUnidadesOrganizacionalesOrganizacionServicioDeDominioViewModel
{
    public Guid? Id_Organizacion { get; set; }
    public IEnumerable<Guid>? UnidadesAsignar { get; set; }
    public IEnumerable<Guid>? UnidadesDesasignar { get; set; }
}
