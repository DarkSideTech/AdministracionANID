// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2026-01-21 15:35:09.313
// -------------------------------------------------
using AUT2Services.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AUT2Services.Infra.Data.Configurations;

internal class EntityProveedorConfiguration : IEntityTypeConfiguration<Proveedor>
{
    public void Configure(EntityTypeBuilder<Proveedor> builder)
    { 
        builder.Property(p => p.Id)
            .HasColumnName("Id") 
            .IsRequired(); 

        builder.HasKey(p => p.Id)
            .HasName("PK_Proveedor_Id"); 
 
        builder.Property(p => p.Codigo)
            .HasColumnName("Codigo") 
            .IsRequired() 
            .HasMaxLength(100); 
 
        builder.Property(p => p.Nombre)
            .HasColumnName("Nombre") 
            .IsRequired() 
            .HasMaxLength(255); 
 
        builder.Property(p => p.Descripcion)
            .HasColumnName("Descripcion") 
            .HasMaxLength(500); 
 
        builder.Property(p => p.APIDeAutenticacion)
            .HasColumnName("APIDeAutenticacion"); 
 
        builder.Property(p => p.ProveedorBase)
            .HasColumnName("ProveedorBase") 
            .IsRequired(); 
 
        builder.Property(p => p.Activo)
            .HasColumnName("Activo") 
            .IsRequired(); 
    }
}

