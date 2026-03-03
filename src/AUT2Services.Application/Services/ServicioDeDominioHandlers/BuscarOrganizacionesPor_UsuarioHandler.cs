using AUT2Services.Application.ViewModels.ServiciosDeDominio;

namespace AUT2Services.Application.Services.ServicioDeDominioHandlers;

public partial class ServicioDeDominioServiceApp
{
    public async Task<IEnumerable<OrganizacionPorUsuarioViewModel>> BuscarOrganizacionesPor_Usuario()
    {
        IList<OrganizacionPorUsuarioViewModel> result = [];

        var resultGet = await servicioDeDominioRepository.BuscarOrganizacionesPor_Id_Usuario(Guid.Parse(userAccessor.GetIdUsuario()));

        if (resultGet.Any())
        {
            foreach (var item in resultGet)
            {
                result.Add(new OrganizacionPorUsuarioViewModel()
                {
                    Codigo_Organizacion = item.Codigo_Organizacion,
                    Nombre_Organizacion = item.Nombre_Organizacion
                });
            }
        }

        return result;
    }
}
