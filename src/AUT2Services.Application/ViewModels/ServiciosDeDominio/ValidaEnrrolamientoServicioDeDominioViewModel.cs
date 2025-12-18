using System.ComponentModel;

namespace AUT2Services.Application.ViewModels.ServiciosDeDominio;

public class ValidaEnrrolamientoServicioDeDominioViewModel
{
    [DisplayName("Id_Usuario_Validado")] 
    public Guid? Id_Usuario_Validado { get; set; } 
     
    [DisplayName("Id_Usuario_Valida_Enrrolamiento")] 
    public Guid? Id_Usuario_Valida_Enrrolamiento { get; set; } 
}

