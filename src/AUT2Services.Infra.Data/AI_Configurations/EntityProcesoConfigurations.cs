// -------------------------------------------------
// Dark Side Tech
// Solution Name : AUT2Services
// Domain : Administracion version 1.17
// Date Generated File : 2026-04-05 15:14:10.212
// -------------------------------------------------
using AUT2Services.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AUT2Services.Infra.Data.Configurations;

internal class EntityProcesoConfiguration : IEntityTypeConfiguration<Proceso>
{
    public void Configure(EntityTypeBuilder<Proceso> builder)
    { 
        builder.Property(p => p.Id)
            .HasColumnName("Id") 
            .IsRequired(); 

        builder.HasKey(p => p.Id)
            .HasName("PK_Proceso_Id"); 
 
        builder.Property(p => p.IdMacro_Proceso)
            .HasColumnName("IdMacro_Proceso") 
            .IsRequired(); 
 
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
 
        builder.Property(p => p.Contexto)
            .HasColumnName("Contexto"); 
 
        builder.Property(p => p.NivelDeProceso)
            .HasColumnName("NivelDeProceso") 
            .IsRequired() 
            .HasMaxLength(100); 
 
        builder.Property(p => p.Url)
            .HasColumnName("Url"); 
 
        builder.Property(p => p.Token)
            .HasColumnName("Token"); 
 
        builder.Property(p => p.ComoDesplegarUrlDeProceso)
            .HasColumnName("ComoDesplegarUrlDeProceso") 
            .IsRequired() 
            .HasMaxLength(100); 
 
        builder.Property(p => p.ProcesoBase)
            .HasColumnName("ProcesoBase") 
            .IsRequired(); 
 
        builder.Property(p => p.MaximaAsignacionDeRoles)
            .HasColumnName("MaximaAsignacionDeRoles") 
            .IsRequired(); 
 
        builder.Property(p => p.Activo)
            .HasColumnName("Activo") 
            .IsRequired(); 
    }
}

