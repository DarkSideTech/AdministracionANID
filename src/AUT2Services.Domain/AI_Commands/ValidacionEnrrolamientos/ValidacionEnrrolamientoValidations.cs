// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2025-12-03 21:03:27.100
// -------------------------------------------------
using AUT2Services.Domain.Core.CommonValidators.Validators;
using AUT2Services.Domain.Enumerations;
using FluentValidation;

namespace AUT2Services.Domain.Commands.ValidacionEnrrolamientos; 

public abstract class ValidacionEnrrolamientoValidations<T> : AbstractValidator<T> where T : ValidacionEnrrolamientoCommand
{
    protected void Validate_Id()
    {
        RuleFor(rf => rf.Id)
            .NotEqual(Guid.Empty) 
            .NotEmpty() 
                .WithMessage("El valor ingresado para el campo Id no puede estar vacio"); 
    } 

    protected void Validate_IdValidado_Usuario()
    {
        RuleFor(rf => rf.IdValidado_Usuario)
            .NotEqual(Guid.Empty) 
            .NotEmpty() 
                .WithMessage("El valor ingresado para el campo IdValidado_Usuario no puede estar vacio"); 
    } 

    protected void Validate_IdValidaEnrrolamiento_Usuario()
    {
        RuleFor(rf => rf.IdValidaEnrrolamiento_Usuario)
            .NotEqual(Guid.Empty) 
            .NotEmpty() 
                .WithMessage("El valor ingresado para el campo IdValidaEnrrolamiento_Usuario no puede estar vacio"); 
    } 

    protected void Validate_EnrrolamientoAceptado()
    {
        RuleFor(rf => rf.EnrrolamientoAceptado)
            .Must(x => x == false || x == true) 
                    .WithMessage("El valor ingresado debe contener un true o false"); 
    } 

    protected void Validate_FechaValidacion()
    {
        RuleFor(rf => rf.FechaValidacion)
            .NotEqual(DateTimeOffset.MinValue) 
            .NotEmpty() 
                .WithMessage("El valor ingresado para el campo FechaValidacion no puede estar vacio") 
            .Must((x, y) => CommonValidator.DateTimeValidator(x.FechaValidacion)) 
                .WithMessage("El valor ingresado debe ser una fecha y hora validos"); 
    } 

    protected void Validate_FechaRegistro()
    {
        RuleFor(rf => rf.FechaRegistro)
            .NotEqual(DateTimeOffset.MinValue) 
            .NotEmpty() 
                .WithMessage("El valor ingresado para el campo FechaRegistro no puede estar vacio") 
            .Must((x, y) => CommonValidator.DateTimeValidator(x.FechaRegistro)) 
                .WithMessage("El valor ingresado debe ser una fecha y hora validos"); 
    } 

    protected void Validate_Activo()
    {
        RuleFor(rf => rf.Activo)
            .Must(x => x == false || x == true) 
                    .WithMessage("El valor ingresado debe contener un true o false"); 
    } 

}

