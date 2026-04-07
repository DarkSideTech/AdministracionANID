// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2026-04-05 15:14:10.131
// -------------------------------------------------
using AUT2Services.Domain.Core.CommonValidators.Validators;
using AUT2Services.Domain.Enumerations;
using FluentValidation;

namespace AUT2Services.Domain.Commands.UnidadesOrganizacionales; 

public abstract class UnidadOrganizacionalValidations<T> : AbstractValidator<T> where T : UnidadOrganizacionalCommand
{
    protected void Validate_Id()
    {
        RuleFor(rf => rf.Id)
            .NotEqual(Guid.Empty) 
            .NotEmpty() 
                .WithMessage("El valor ingresado para el campo Id no puede estar vacio"); 
    } 

    protected void Validate_Id_Organizacion()
    {
        RuleFor(rf => rf.Id_Organizacion)
            .NotEqual(Guid.Empty) 
            .NotEmpty() 
                .WithMessage("El valor ingresado para el campo Id_Organizacion no puede estar vacio"); 
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

    protected void Validate_UnidadOrganizacionalBase()
    {
        RuleFor(rf => rf.UnidadOrganizacionalBase)
            .Must(x => x == false || x == true) 
                    .WithMessage("El valor ingresado debe contener un true o false"); 
    } 

    protected void Validate_Activo()
    {
        RuleFor(rf => rf.Activo)
            .Must(x => x == false || x == true) 
                    .WithMessage("El valor ingresado debe contener un true o false"); 
    } 

}

