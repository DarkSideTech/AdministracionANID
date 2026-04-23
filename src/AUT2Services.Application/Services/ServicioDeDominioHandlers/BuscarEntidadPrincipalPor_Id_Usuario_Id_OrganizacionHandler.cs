using AUT2Services.Application.ViewModels;

namespace AUT2Services.Application.Services.ServicioDeDominioHandlers;

public partial class ServicioDeDominioServiceApp
{
    public async Task<EntidadViewModel?> BuscarEntidadPrincipalPor_Id_Usuario_Id_Organizacion(Guid id_Usuario, Guid id_Organizacion)
    {
        EntidadViewModel? result = null;

        var resultGet = await servicioDeDominioRepository.BuscarEntidadPrincipalPor_Id_Usuario_Id_Organizacion(
            id_Usuario,
            id_Organizacion
        );

        if (resultGet is not null)
        {
            result = new EntidadViewModel()
            {
                Id = resultGet.Id,
                Id_UnidadOrganizacional = resultGet.Id_UnidadOrganizacional,
                Id_Usuario = resultGet.Id_Usuario,
                TipoDeEntidad = resultGet.TipoDeEntidad,
                CorreoElectronico = resultGet.CorreoElectronico,
                FechaInicioAutorizacion = resultGet.FechaInicioAutorizacion,
                FechaTerminoAutorizacion = resultGet.FechaTerminoAutorizacion,
                FechaCreacion = resultGet.FechaCreacion,
                Principal = resultGet.Principal,
                EntidadBase = resultGet.EntidadBase
            };
        }

        return result;
    }
}
