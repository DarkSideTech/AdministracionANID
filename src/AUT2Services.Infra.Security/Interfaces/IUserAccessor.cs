namespace AUT2Services.Infra.Security.Interfaces;

public interface IUserAccessor
{
    string GetUsername();
    string GetEmail();
    string GetIdEntidad();
    List<string> GetProcesos();
    List<string> GetRolesPorProceso(string proceso);
    string GetIdUsuario();
    string GetNombreADesplegar();
    string GetCodigoOrganizacion();
    string GetNombreOrganizacion();
    string GetCodigoUnidadOrganizacional();
    string GetNombreUnidadOrganizacional();
    string GetAccessTokenType();
}