// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2025-12-03 21:03:27.142
// -------------------------------------------------
using AUT2Services.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AUT2Services.Infra.Data.Configurations;

internal class EntityOrganizacionConfiguration : IEntityTypeConfiguration<Organizacion>
{
    public void Configure(EntityTypeBuilder<Organizacion> builder)
    { 
        builder.Property(p => p.Id)
            .HasColumnName("Id") 
            .IsRequired(); 

        builder.HasKey(p => p.Id)
            .HasName("PK_Organizacion_Id"); 
 
        builder.Property(p => p.IdOrganizacion)
            .HasColumnName("IdOrganizacion"); 
 
        builder.Property(p => p.Codigo)
            .HasColumnName("Codigo") 
            .IsRequired() 
            .HasMaxLength(100); 
 
        builder.Property(p => p.Nombre)
            .HasColumnName("Nombre") 
            .IsRequired() 
            .HasMaxLength(255); 
 
        builder.Property(p => p.Descripcion)
            .HasColumnName("Descripcion"); 
 
        builder.Property(p => p.OrganizacionBase)
            .HasColumnName("OrganizacionBase") 
            .IsRequired(); 
 
        builder.Property(p => p.Activo)
            .HasColumnName("Activo") 
            .IsRequired(); 
    }
}

