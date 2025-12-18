// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2025-12-03 21:03:27.141
// -------------------------------------------------
using AUT2Services.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AUT2Services.Infra.Data.Configurations;

internal class EntityValidacionEnrrolamientoConfiguration : IEntityTypeConfiguration<ValidacionEnrrolamiento>
{
    public void Configure(EntityTypeBuilder<ValidacionEnrrolamiento> builder)
    { 
        builder.Property(p => p.Id)
            .HasColumnName("Id") 
            .IsRequired(); 

        builder.HasKey(p => p.Id)
            .HasName("PK_ValidacionEnrrolamiento_Id"); 
 
        builder.Property(p => p.IdValidado_Usuario)
            .HasColumnName("IdValidado_Usuario") 
            .IsRequired(); 
 
        builder.Property(p => p.IdValidaEnrrolamiento_Usuario)
            .HasColumnName("IdValidaEnrrolamiento_Usuario") 
            .IsRequired(); 
 
        builder.Property(p => p.EnrrolamientoAceptado)
            .HasColumnName("EnrrolamientoAceptado") 
            .IsRequired(); 
 
        builder.Property(p => p.FechaValidacion)
            .HasColumnName("FechaValidacion") 
            .IsRequired(); 
 
        builder.Property(p => p.FechaRegistro)
            .HasColumnName("FechaRegistro") 
            .IsRequired(); 
 
        builder.Property(p => p.Activo)
            .HasColumnName("Activo") 
            .IsRequired(); 
    }
}

