using AUT2Services.Application.ViewModels;

namespace AUT2Services.Application.Services.ServicioDeDominioHandlers;

public partial class ServicioDeDominioServiceApp
{
    public async Task<IEnumerable<UnidadOrganizacionalViewModel>> BuscarUnidadesOrganizacionalesPor_Id_Usuario_Id_Organizacion(Guid id_Usuario, Guid id_Organizacion)
    {
        IList<UnidadOrganizacionalViewModel> result = [];

        var resultGet = await servicioDeDominioRepository.BuscarUnidadesOrganizacionalesPor_Id_Usuario_Id_Organizacion(
             id_Usuario,
             id_Organizacion
         );

        if (resultGet.Any())
        {
            foreach (var item in resultGet)
            {
                result.Add(new UnidadOrganizacionalViewModel()
                {
                    Id = item.Id,
                    Id_Organizacion = item.Id_Organizacion,
                    Codigo = item.Codigo,
                    Nombre = item.Nombre,
                    Descripcion = item.Descripcion,
                    UnidadOrganizacionalBase = item.UnidadOrganizacionalBase,
                    Activo = item.Activo
                });
            }
        }

        return result;
    }
}
