// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2026-04-05 15:14:10.211
// -------------------------------------------------
using AUT2Services.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AUT2Services.Infra.Data.Configurations;

internal class EntityAutenticadorExternoConfiguration : IEntityTypeConfiguration<AutenticadorExterno>
{
    public void Configure(EntityTypeBuilder<AutenticadorExterno> builder)
    { 
        builder.Property(p => p.Id)
            .HasColumnName("Id") 
            .IsRequired(); 

        builder.HasKey(p => p.Id)
            .HasName("PK_AutenticadorExterno_Id"); 
 
        builder.Property(p => p.Id_Proveedor)
            .HasColumnName("Id_Proveedor") 
            .IsRequired(); 
 
        builder.Property(p => p.Id_Usuario)
            .HasColumnName("Id_Usuario") 
            .IsRequired(); 
 
        builder.Property(p => p.NombreUsuario)
            .HasColumnName("NombreUsuario") 
            .IsRequired() 
            .HasMaxLength(255); 
 
        builder.Property(p => p.ClaveDeAcceso)
            .HasColumnName("ClaveDeAcceso") 
            .IsRequired(); 
 
        builder.Property(p => p.NombreADesplegar)
            .HasColumnName("NombreADesplegar") 
            .IsRequired(); 
 
        builder.Property(p => p.ValidadorPrimario)
            .HasColumnName("ValidadorPrimario") 
            .IsRequired(); 
 
        builder.Property(p => p.AutenticadorExternoBase)
            .HasColumnName("AutenticadorExternoBase"); 
 
        builder.Property(p => p.Activo)
            .HasColumnName("Activo") 
            .IsRequired(); 
    }
}

