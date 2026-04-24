namespace AUT2Services.Infra.Security.Accounts.BuscarUsuariosPaginados;

public class BuscarUsuariosPaginadosCommandValidations : BuscarUsuariosPaginadosValidations<BuscarUsuariosPaginadosCommand>
{
    public BuscarUsuariosPaginadosCommandValidations()
    {
        Validate_NumeroDePagina();
        Validate_CantidadPorPagina();
        Validate_Busqueda();
    }
}
