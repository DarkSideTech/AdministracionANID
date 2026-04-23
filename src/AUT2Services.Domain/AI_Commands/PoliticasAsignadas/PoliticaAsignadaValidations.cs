// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2026-04-05 15:14:10.146
// -------------------------------------------------
using AUT2Services.Domain.Core.CommonValidators.Validators;
using AUT2Services.Domain.Enumerations;
using FluentValidation;

namespace AUT2Services.Domain.Commands.PoliticasAsignadas; 

public abstract class PoliticaAsignadaValidations<T> : AbstractValidator<T> where T : PoliticaAsignadaCommand
{
    protected void Validate_Id()
    {
        RuleFor(rf => rf.Id)
            .NotEqual(Guid.Empty) 
            .NotEmpty() 
                .WithMessage("El valor ingresado para el campo Id no puede estar vacio"); 
    } 

    protected void Validate_Id_Entidad()
    {
        RuleFor(rf => rf.Id_Entidad)
            .NotEqual(Guid.Empty) 
            .NotEmpty() 
                .WithMessage("El valor ingresado para el campo Id_Entidad no puede estar vacio"); 
    } 

    protected void Validate_Id_Rol()
    {
        RuleFor(rf => rf.Id_Rol)
            .NotEqual(Guid.Empty) 
            .NotEmpty() 
                .WithMessage("El valor ingresado para el campo Id_Rol no puede estar vacio"); 
    } 

    protected void Validate_Id_Proceso()
    {
        RuleFor(rf => rf.Id_Proceso)
            .NotEqual(Guid.Empty) 
            .NotEmpty() 
                .WithMessage("El valor ingresado para el campo Id_Proceso no puede estar vacio"); 
    } 

    protected void Validate_FechaInicioAsignacion()
    {
        RuleFor(rf => rf.FechaInicioAsignacion)
            .NotEmpty() 
                .WithMessage("El valor ingresado para el campo FechaInicioAsignacion no puede estar vacio") 
            .Must((x, y) => CommonValidator.DateTimeValidator(x.FechaInicioAsignacion!)) 
                .WithMessage("El valor ingresado debe ser una fecha y hora validos"); 
    } 

    protected void Validate_FechaTerminoAsignacion()
    {
        RuleFor(rf => rf.FechaTerminoAsignacion)
            .NotEmpty() 
                .WithMessage("El valor ingresado para el campo FechaTerminoAsignacion no puede estar vacio") 
            .Must((x, y) => CommonValidator.DateTimeValidator(x.FechaTerminoAsignacion!)) 
                .WithMessage("El valor ingresado debe ser una fecha y hora validos"); 
    } 

    protected void Validate_FechaCreacion()
    {
        RuleFor(rf => rf.FechaCreacion)
            .NotEmpty() 
                .WithMessage("El valor ingresado para el campo FechaCreacion no puede estar vacio") 
            .Must((x, y) => CommonValidator.DateTimeValidator(x.FechaCreacion!)) 
                .WithMessage("El valor ingresado debe ser una fecha y hora validos"); 
    } 

    protected void Validate_RolRequiereValidacion()
    {
        RuleFor(rf => rf.RolRequiereValidacion)
            .Must(x => x == false || x == true) 
                    .WithMessage("El valor ingresado debe contener un true o false"); 
    } 

    protected void Validate_RolAsignadoValidado()
    {
        RuleFor(rf => rf.RolAsignadoValidado)
            .Must(x => x == false || x == true) 
                    .WithMessage("El valor ingresado debe contener un true o false"); 
    } 

    protected void Validate_PoliticaAsignadaBase()
    {
        RuleFor(rf => rf.PoliticaAsignadaBase)
            .Must(x => x == false || x == true) 
                    .WithMessage("El valor ingresado debe contener un true o false"); 
    } 

}

