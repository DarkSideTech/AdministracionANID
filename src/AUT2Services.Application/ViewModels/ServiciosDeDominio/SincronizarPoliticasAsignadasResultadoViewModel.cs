namespace AUT2Services.Application.ViewModels.ServiciosDeDominio;

public class SincronizarPoliticasAsignadasResultadoViewModel
{
    public int Creadas { get; set; }
    public int Eliminadas { get; set; }
    public int OmitidasPorExistir { get; set; }
    public int OmitidasPorNoExistir { get; set; }
    public int OmitidasPorError { get; set; }
    public IList<string> Errores { get; set; } = [];
}
