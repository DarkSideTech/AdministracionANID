// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2026-01-21 15:35:09.281
// -------------------------------------------------
using AUT2Services.Domain.Core.CommonValidators.Validators;
using AUT2Services.Domain.Enumerations;
using FluentValidation;

namespace AUT2Services.Domain.Commands.Procesos; 

public abstract class ProcesoValidations<T> : AbstractValidator<T> where T : ProcesoCommand
{
    protected void Validate_Id()
    {
        RuleFor(rf => rf.Id)
            .NotEqual(Guid.Empty) 
            .NotEmpty() 
                .WithMessage("El valor ingresado para el campo Id no puede estar vacio"); 
    } 

    protected void Validate_IdMacro_Proceso()
    {
        RuleFor(rf => rf.IdMacro_Proceso)
            .NotEqual(Guid.Empty) 
            .NotEmpty() 
                .WithMessage("El valor ingresado para el campo IdMacro_Proceso no puede estar vacio"); 
    } 

    protected void Validate_Codigo()
    {
        RuleFor(rf => rf.Codigo)
            .NotEmpty() 
                .WithMessage("El valor ingresado para el campo Codigo no puede estar vacio") 
            .Length(3, 100) 
                .WithMessage("El valor ingresado debe contener entre 3 y 100 caracteres"); 
    } 

    protected void Validate_Nombre()
    {
        RuleFor(rf => rf.Nombre)
            .NotEmpty() 
                .WithMessage("El valor ingresado para el campo Nombre no puede estar vacio") 
            .Length(3, 255) 
                .WithMessage("El valor ingresado debe contener entre 3 y 255 caracteres"); 
    } 

    protected void Validate_Descripcion()
    {
        RuleFor(rf => rf.Descripcion)
            .NotEmpty() 
                .WithMessage("El valor ingresado para el campo Descripcion no puede estar vacio"); 
    } 

    protected void Validate_Contexto()
    {
        RuleFor(rf => rf.Contexto)
            .NotEmpty() 
                .WithMessage("El valor ingresado para el campo Contexto no puede estar vacio"); 
    } 

    protected void Validate_NivelDeProceso()
    {
        RuleFor(rf => rf.NivelDeProceso)
            .Must((x, y) => CommonValidator.EnumerationValidator(typeof(EnumNivelDeProceso), x.NivelDeProceso)) 
                .WithMessage("El valor ingresado para el campo NivelDeProceso debe ser un valor valido definido en la enumeracion") 
            .Length(3, 100) 
                .WithMessage("El valor ingresado debe contener entre 3 y 100 caracteres"); 
    } 

    protected void Validate_Url()
    {
        RuleFor(rf => rf.Url)
            .NotEmpty() 
                .WithMessage("El valor ingresado para el campo Url no puede estar vacio"); 
    } 

    protected void Validate_Token()
    {
        RuleFor(rf => rf.Token)
            .NotEmpty() 
                .WithMessage("El valor ingresado para el campo Token no puede estar vacio"); 
    } 

    protected void Validate_ComoDesplegarUrlDeProceso()
    {
        RuleFor(rf => rf.ComoDesplegarUrlDeProceso)
            .Must((x, y) => CommonValidator.EnumerationValidator(typeof(EnumComoDesplegarUrlDeProceso), x.ComoDesplegarUrlDeProceso)) 
                .WithMessage("El valor ingresado para el campo ComoDesplegarUrlDeProceso debe ser un valor valido definido en la enumeracion") 
            .Length(3, 100) 
                .WithMessage("El valor ingresado debe contener entre 3 y 100 caracteres"); 
    } 

    protected void Validate_ProcesoBase()
    {
        RuleFor(rf => rf.ProcesoBase)
            .Must(x => x == false || x == true) 
                    .WithMessage("El valor ingresado debe contener un true o false"); 
    } 

    protected void Validate_MaximaAsignacionDeRoles()
    {
        RuleFor(rf => rf.MaximaAsignacionDeRoles)
            .NotEmpty() 
                .WithMessage("El valor ingresado para el campo MaximaAsignacionDeRoles no puede estar vacio"); 
    } 

    protected void Validate_Activo()
    {
        RuleFor(rf => rf.Activo)
            .Must(x => x == false || x == true) 
                    .WithMessage("El valor ingresado debe contener un true o false"); 
    } 

}

