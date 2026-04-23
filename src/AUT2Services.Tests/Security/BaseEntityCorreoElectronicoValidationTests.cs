using AUT2Services.Domain.Commands.Entidades.Commands;
using AUT2Services.Domain.Enumerations;
using AUT2Services.Infra.Security.Accounts.BaseEntity;

namespace AUT2Services.Tests.Security;

[TestClass]
public class BaseEntityCorreoElectronicoValidationTests
{
    [TestMethod]
    public void BaseEntityCommand_WhenCorreoElectronicoEsVacioYSePermite_DebeSerValido()
    {
        var command = new BaseEntityCommand
        {
            CodigoOrganizacion = "ORG_TEST",
            NombreOrganizacion = "Organizacion Test",
            Id_Usuario = Guid.NewGuid(),
            TipoDeEntidad = EnumTipoDeEntidad.PERSONA,
            CorreoElectronico = string.Empty,
            PermitirCorreoElectronicoVacio = true
        };

        var isValid = command.IsValid();

        Assert.IsTrue(isValid);
        Assert.AreEqual(0, command.CommandResponse.ValidationResult.Errors.Count);
    }

    [TestMethod]
    public void CrearEntidadCommand_WhenCorreoElectronicoEsVacioYSePermite_DebeSerValido()
    {
        var command = new CrearEntidadCommand(
            Guid.NewGuid(),
            Guid.NewGuid(),
            EnumTipoDeEntidad.PERSONA,
            string.Empty,
            permitirCorreoElectronicoVacio: true);

        var isValid = command.IsValid();

        Assert.IsTrue(isValid);
        Assert.AreEqual(0, command.CommandResponse.ValidationResult.Errors.Count);
    }

    [TestMethod]
    public void CrearEntidadCommand_WhenCorreoElectronicoEsVacioYSinPermiso_DebeSerInvalido()
    {
        var command = new CrearEntidadCommand(
            Guid.NewGuid(),
            Guid.NewGuid(),
            EnumTipoDeEntidad.PERSONA,
            string.Empty);

        var isValid = command.IsValid();

        Assert.IsFalse(isValid);
        Assert.IsTrue(command.CommandResponse.ValidationResult.Errors.Any(error =>
            error.ErrorMessage.Contains("CorreoElectronico no puede estar vacio")));
    }
}
