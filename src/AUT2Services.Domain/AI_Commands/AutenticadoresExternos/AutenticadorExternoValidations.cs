// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2026-01-21 15:35:09.249
// -------------------------------------------------
using AUT2Services.Domain.Core.CommonValidators.Validators;
using AUT2Services.Domain.Enumerations;
using FluentValidation;

namespace AUT2Services.Domain.Commands.AutenticadoresExternos; 

public abstract class AutenticadorExternoValidations<T> : AbstractValidator<T> where T : AutenticadorExternoCommand
{
    protected void Validate_Id()
    {
        RuleFor(rf => rf.Id)
            .NotEqual(Guid.Empty) 
            .NotEmpty() 
                .WithMessage("El valor ingresado para el campo Id no puede estar vacio"); 
    } 

    protected void Validate_Id_Proveedor()
    {
        RuleFor(rf => rf.Id_Proveedor)
            .NotEqual(Guid.Empty) 
            .NotEmpty() 
                .WithMessage("El valor ingresado para el campo Id_Proveedor no puede estar vacio"); 
    } 

    protected void Validate_Id_Usuario()
    {
        RuleFor(rf => rf.Id_Usuario)
            .NotEqual(Guid.Empty) 
            .NotEmpty() 
                .WithMessage("El valor ingresado para el campo Id_Usuario no puede estar vacio"); 
    } 

    protected void Validate_NombreUsuario()
    {
        RuleFor(rf => rf.NombreUsuario)
            .NotEmpty() 
                .WithMessage("El valor ingresado para el campo NombreUsuario no puede estar vacio") 
            .Length(3, 255) 
                .WithMessage("El valor ingresado debe contener entre 3 y 255 caracteres"); 
    } 

    protected void Validate_ClaveDeAcceso()
    {
        RuleFor(rf => rf.ClaveDeAcceso)
            .NotEmpty() 
                .WithMessage("El valor ingresado para el campo ClaveDeAcceso no puede estar vacio"); 
    } 

    protected void Validate_NombreADesplegar()
    {
        RuleFor(rf => rf.NombreADesplegar)
            .NotEmpty() 
                .WithMessage("El valor ingresado para el campo NombreADesplegar no puede estar vacio"); 
    } 

    protected void Validate_ValidadorPrimario()
    {
        RuleFor(rf => rf.ValidadorPrimario)
            .Must(x => x == false || x == true) 
                    .WithMessage("El valor ingresado debe contener un true o false"); 
    } 

    protected void Validate_AutenticadorExternoBase()
    {
        RuleFor(rf => rf.AutenticadorExternoBase)
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

