namespace AUT2Services.Application.ViewModels.ServiciosDeDominio;

public sealed class SincronizarUnidadesOrganizacionalesOrganizacionResultadoViewModel
{
    public int Asignadas { get; set; }
    public int Reasignadas { get; set; }
    public int EntidadesEliminadas { get; set; }
    public int PoliticasAsignadasEliminadas { get; set; }
    public int OmitidasPorExistir { get; set; }
    public int OmitidasPorError { get; set; }
    public IList<string> Errores { get; set; } = [];
}
