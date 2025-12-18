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

internal class EntityEntidadConfiguration : IEntityTypeConfiguration<Entidad>
{
    public void Configure(EntityTypeBuilder<Entidad> builder)
    { 
        builder.Property(p => p.Id)
            .HasColumnName("Id") 
            .IsRequired(); 

        builder.HasKey(p => p.Id)
            .HasName("PK_Entidad_Id"); 
 
        builder.Property(p => p.Id_UnidadOrganizacional)
            .HasColumnName("Id_UnidadOrganizacional") 
            .IsRequired(); 
 
        builder.Property(p => p.Id_Usuario)
            .HasColumnName("Id_Usuario") 
            .IsRequired(); 
 
        builder.Property(p => p.TipoDeEntidad)
            .HasColumnName("TipoDeEntidad") 
            .IsRequired() 
            .HasMaxLength(100); 
 
        builder.Property(p => p.CorreoElectronico)
            .HasColumnName("CorreoElectronico") 
            .IsRequired(); 
 
        builder.Property(p => p.FechaInicioAutorizacion)
            .HasColumnName("FechaInicioAutorizacion") 
            .IsRequired(); 
 
        builder.Property(p => p.FechaTerminoAutorizacion)
            .HasColumnName("FechaTerminoAutorizacion") 
            .IsRequired(); 
 
        builder.Property(p => p.FechaCreacion)
            .HasColumnName("FechaCreacion") 
            .IsRequired(); 
 
        builder.Property(p => p.Principal)
            .HasColumnName("Principal") 
            .IsRequired(); 
 
        builder.Property(p => p.EntidadBase)
            .HasColumnName("EntidadBase") 
            .IsRequired(); 
    }
}

