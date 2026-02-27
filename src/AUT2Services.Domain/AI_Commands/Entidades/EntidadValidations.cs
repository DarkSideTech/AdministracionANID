// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2026-01-21 15:35:09.264
// -------------------------------------------------
using AUT2Services.Domain.Core.CommonValidators.Validators;
using AUT2Services.Domain.Enumerations;
using FluentValidation;

namespace AUT2Services.Domain.Commands.Entidades; 

public abstract class EntidadValidations<T> : AbstractValidator<T> where T : EntidadCommand
{
    protected void Validate_Id()
    {
        RuleFor(rf => rf.Id)
            .NotEqual(Guid.Empty) 
            .NotEmpty() 
                .WithMessage("El valor ingresado para el campo Id no puede estar vacio"); 
    } 

    protected void Validate_Id_UnidadOrganizacional()
    {
        RuleFor(rf => rf.Id_UnidadOrganizacional)
            .NotEqual(Guid.Empty) 
            .NotEmpty() 
                .WithMessage("El valor ingresado para el campo Id_UnidadOrganizacional no puede estar vacio"); 
    } 

    protected void Validate_Id_Usuario()
    {
        RuleFor(rf => rf.Id_Usuario)
            .NotEqual(Guid.Empty) 
            .NotEmpty() 
                .WithMessage("El valor ingresado para el campo Id_Usuario no puede estar vacio"); 
    } 

    protected void Validate_TipoDeEntidad()
    {
        RuleFor(rf => rf.TipoDeEntidad)
            .Must((x, y) => CommonValidator.EnumerationValidator(typeof(EnumTipoDeEntidad), x.TipoDeEntidad)) 
                .WithMessage("El valor ingresado para el campo TipoDeEntidad debe ser un valor valido definido en la enumeracion") 
            .Length(3, 100) 
                .WithMessage("El valor ingresado debe contener entre 3 y 100 caracteres"); 
    } 

    protected void Validate_CorreoElectronico()
    {
        RuleFor(rf => rf.CorreoElectronico)
            .NotEmpty() 
                .WithMessage("El valor ingresado para el campo CorreoElectronico no puede estar vacio") 
            .EmailAddress() 
                .WithMessage("El correo electronico ingresado no es valido"); 
    } 

    protected void Validate_FechaInicioAutorizacion()
    {
        RuleFor(rf => rf.FechaInicioAutorizacion)
            .NotEqual(DateTimeOffset.MinValue) 
            .NotEmpty() 
                .WithMessage("El valor ingresado para el campo FechaInicioAutorizacion no puede estar vacio") 
            .Must((x, y) => CommonValidator.DateTimeValidator(x.FechaInicioAutorizacion)) 
                .WithMessage("El valor ingresado debe ser una fecha y hora validos"); 
    } 

    protected void Validate_FechaTerminoAutorizacion()
    {
        RuleFor(rf => rf.FechaTerminoAutorizacion)
            .NotEqual(DateTimeOffset.MinValue) 
            .NotEmpty() 
                .WithMessage("El valor ingresado para el campo FechaTerminoAutorizacion no puede estar vacio") 
            .Must((x, y) => CommonValidator.DateTimeValidator(x.FechaTerminoAutorizacion)) 
                .WithMessage("El valor ingresado debe ser una fecha y hora validos"); 
    } 

    protected void Validate_FechaCreacion()
    {
        RuleFor(rf => rf.FechaCreacion)
            .NotEqual(DateTimeOffset.MinValue) 
            .NotEmpty() 
                .WithMessage("El valor ingresado para el campo FechaCreacion no puede estar vacio") 
            .Must((x, y) => CommonValidator.DateTimeValidator(x.FechaCreacion)) 
                .WithMessage("El valor ingresado debe ser una fecha y hora validos"); 
    } 

    protected void Validate_Principal()
    {
        RuleFor(rf => rf.Principal)
            .Must(x => x == false || x == true) 
                    .WithMessage("El valor ingresado debe contener un true o false"); 
    } 

    protected void Validate_EntidadBase()
    {
        RuleFor(rf => rf.EntidadBase)
            .Must(x => x == false || x == true) 
                    .WithMessage("El valor ingresado debe contener un true o false"); 
    } 

}

