using AUT2Services.Domain.Enumerations;
using AUT2Services.Domain.Security.Entities;
using AUT2Services.Infra.Security.Accounts.ModificaUsuario;
using AUT2Services.Infra.Security.Enumerations;
using AUT2Services.Infra.Security.Models;
using AUT2Services.Tests.Support;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging.Abstractions;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace AUT2Services.Tests.Security;

[TestClass]
public class ModificaUsuarioCommandHandlerTests
{
    [TestMethod]
    public async Task Handle_ForForeignUser_UpdatesMutableFieldsAndPreservesImmutableFields()
    {
        using var identityHost = new IdentityTestHost(nameof(Handle_ForForeignUser_UpdatesMutableFieldsAndPreservesImmutableFields));
        var usuario = await CreateUserAsync(identityHost, "foreign-update@example.com", EnumTipoDeUsuario.EXTRANJERO);
        var originalInfo = usuario.InformacionAdicional.ToInformacionAdicionalModel();

        var handler = CreateHandler(identityHost, usuario.Id);
        var command = CreateCommand(usuario.Id, usuario.Email!, EnumTipoDeUsuario.EXTRANJERO);
        command.Nacionalidad = EnumNacionalidad.Argentino_a;
        command.NumeroDeTelefono = "+56912345678";
        command.DocumentoDeIdentidad = "DNI";
        command.NumeroDeDocumento = "99887766";
        command.CodigoValidadorDocumento = "K";
        command.PrimerNombre = "Marcelo";
        command.SegundoNombre = "Andres";
        command.PrimerApellido = "Vega";
        command.SegundoApellido = "Janhsen";
        command.SexoDeclarativo = "HOMBRE";
        command.SexoRegistral = "MASCULINO";
        command.FechaDeNacimiento = new DateOnly(1990, 10, 20);
        command.CorreoElectronico = "otro-correo@example.com";
        command.TipoDeUsuario = EnumTipoDeUsuario.NACIONAL;
        command.Contraseña = "OtraClave123$";
        command.ConfirmaContraseña = "OtraClave123$";
        command.TerminosYCondiciones = false;

        var response = await handler.Handle(command, CancellationToken.None);

        Assert.IsTrue(response.Result);

        var refreshedUser = await identityHost.UserManager.FindByIdAsync(usuario.Id);
        Assert.IsNotNull(refreshedUser);
        Assert.AreEqual("foreign-update@example.com", refreshedUser!.Email);
        Assert.AreEqual(EnumTipoDeUsuario.EXTRANJERO, refreshedUser.TipoDeUsuario);
        Assert.AreEqual("+56912345678", refreshedUser.PhoneNumber);

        var updatedInfo = refreshedUser.InformacionAdicional.ToInformacionAdicionalModel();
        Assert.AreEqual(EnumNacionalidad.Argentino_a, updatedInfo.Nacionalidad);
        Assert.AreEqual("DNI", updatedInfo.DocumentoDeIdentidad);
        Assert.AreEqual("99887766", updatedInfo.NumeroDeDocumento);
        Assert.AreEqual("K", updatedInfo.CodigoValidadorDocumento);
        Assert.AreEqual("Marcelo", updatedInfo.PrimerNombre);
        Assert.AreEqual("Andres", updatedInfo.SegundoNombre);
        Assert.AreEqual("Vega", updatedInfo.PrimerApellido);
        Assert.AreEqual("Janhsen", updatedInfo.SegundoApellido);
        Assert.AreEqual("HOMBRE", updatedInfo.SexoDeclarativo);
        Assert.AreEqual("MASCULINO", updatedInfo.SexoRegistral);
        Assert.AreEqual(1, identityHost.DbContext.AuditOutboxMessages.Count(message => message.EventType == "UsuarioModificado"));
        Assert.AreEqual(new DateOnly(1990, 10, 20), updatedInfo.FechaDeNacimiento);
        Assert.AreEqual(originalInfo.TerminosYCondiciones, updatedInfo.TerminosYCondiciones);
    }

    [TestMethod]
    public async Task Handle_ForNationalUser_OnlyUpdatesSexoRegistral()
    {
        using var identityHost = new IdentityTestHost(nameof(Handle_ForNationalUser_OnlyUpdatesSexoRegistral));
        var usuario = await CreateUserAsync(identityHost, "national-update@example.com", EnumTipoDeUsuario.NACIONAL);
        var originalInfo = usuario.InformacionAdicional.ToInformacionAdicionalModel();

        var handler = CreateHandler(identityHost, usuario.Id);
        var command = CreateCommand(usuario.Id, usuario.Email!, EnumTipoDeUsuario.NACIONAL);
        command.NumeroDeTelefono = "+56900000000";
        command.PrimerNombre = "NoDebeCambiar";
        command.PrimerApellido = "NoDebeCambiar";
        command.SexoDeclarativo = "HOMBRE";
        command.SexoRegistral = "NO_BINARIO";

        var response = await handler.Handle(command, CancellationToken.None);

        Assert.IsTrue(response.Result);

        var refreshedUser = await identityHost.UserManager.FindByIdAsync(usuario.Id);
        Assert.IsNotNull(refreshedUser);
        Assert.AreEqual(usuario.PhoneNumber, refreshedUser!.PhoneNumber);

        var updatedInfo = refreshedUser.InformacionAdicional.ToInformacionAdicionalModel();
        Assert.AreEqual(originalInfo.PrimerNombre, updatedInfo.PrimerNombre);
        Assert.AreEqual(originalInfo.PrimerApellido, updatedInfo.PrimerApellido);
        Assert.AreEqual(originalInfo.SexoDeclarativo, updatedInfo.SexoDeclarativo);
        Assert.AreEqual("NO_BINARIO", updatedInfo.SexoRegistral);
        Assert.AreEqual(1, identityHost.DbContext.AuditOutboxMessages.Count(message => message.EventType == "UsuarioModificado"));
    }

    private static ModificaUsuarioCommandHandler CreateHandler(IdentityTestHost identityHost, string userId)
    {
        var principal = new ClaimsPrincipal(new ClaimsIdentity(
        [
            new Claim(ClaimTypes.NameIdentifier, userId),
            new Claim(JwtRegisteredClaimNames.Sid, "test-session-id"),
            new Claim(EnumTokenValidationClaims.SecurityStamp, "security-stamp")
        ], "TestAuth"));

        return new ModificaUsuarioCommandHandler(
            identityHost.UserManager,
            identityHost.DbContext,
            new StubCsrfService(),
            new StubCurrentUserService(userId: userId, principal: principal),
            new StubSessionValidationService(),
            identityHost.SecurityTraceabilityService,
            NullLogger<ModificaUsuarioCommandHandler>.Instance);
    }

    private static ModificaUsuarioCommand CreateCommand(string userId, string email, string tipoDeUsuario)
    {
        var httpContext = new DefaultHttpContext();
        return new ModificaUsuarioCommand
        {
            IdUsuario = userId,
            CorreoElectronico = email,
            TipoDeUsuario = tipoDeUsuario,
            Nacionalidad = EnumNacionalidad.Chileno_a,
            DocumentoDeIdentidad = "PASAPORTE",
            NumeroDeDocumento = "12345678",
            CodigoValidadorDocumento = "9",
            PrimerNombre = "Test",
            SegundoNombre = "Middle",
            PrimerApellido = "User",
            SegundoApellido = "Last",
            SexoDeclarativo = "MUJER",
            SexoRegistral = "FEMENINO",
            FechaDeNacimiento = new DateOnly(1995, 1, 15),
            TerminosYCondiciones = true,
            Request = httpContext.Request,
            Response = httpContext.Response
        };
    }

    private static async Task<Usuario> CreateUserAsync(IdentityTestHost host, string email, string tipoDeUsuario)
    {
        var user = new Usuario
        {
            UserName = email,
            Email = email,
            EmailConfirmed = true,
            PhoneNumber = "+56911111111",
            NombreADesplegar = "Nombre Inicial",
            TipoDeUsuario = tipoDeUsuario,
            Activo = true,
            UsuarioBase = false,
            EstadoDeUsuario = EnumEstadoDeUsuario.REGISTRADO,
            InformacionAdicional = new InformacionAdicionalModel
            {
            Nacionalidad = EnumNacionalidad.Chileno_a,
                DocumentoDeIdentidad = "PASAPORTE",
                NumeroDeDocumento = "11111111",
                CodigoValidadorDocumento = "1",
                PrimerNombre = "Nombre",
                SegundoNombre = "Segundo",
                PrimerApellido = "Apellido",
                SegundoApellido = "SegundoApellido",
                SexoDeclarativo = "MUJER",
                SexoRegistral = "FEMENINO",
                FechaDeNacimiento = new DateOnly(1990, 1, 1),
                TerminosYCondiciones = true
            }.ToJson()
        };

        var result = await host.UserManager.CreateAsync(user, "Changeme123#");
        Assert.IsTrue(result.Succeeded);

        var createdUser = await host.UserManager.FindByEmailAsync(email);
        Assert.IsNotNull(createdUser);
        return createdUser!;
    }
}
