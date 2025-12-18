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

internal class EntityPoliticaAsignadaConfiguration : IEntityTypeConfiguration<PoliticaAsignada>
{
    public void Configure(EntityTypeBuilder<PoliticaAsignada> builder)
    { 
        builder.Property(p => p.Id)
            .HasColumnName("Id") 
            .IsRequired(); 

        builder.HasKey(p => p.Id)
            .HasName("PK_PoliticaAsignada_Id"); 
 
        builder.Property(p => p.Id_Entidad)
            .HasColumnName("Id_Entidad") 
            .IsRequired(); 
 
        builder.Property(p => p.Id_Rol)
            .HasColumnName("Id_Rol") 
            .IsRequired(); 
 
        builder.Property(p => p.Id_Proceso)
            .HasColumnName("Id_Proceso") 
            .IsRequired(); 
 
        builder.Property(p => p.FechaInicioAsignacion)
            .HasColumnName("FechaInicioAsignacion") 
            .IsRequired(); 
 
        builder.Property(p => p.FechaTerminoAsignacion)
            .HasColumnName("FechaTerminoAsignacion") 
            .IsRequired(); 
 
        builder.Property(p => p.FechaCreacion)
            .HasColumnName("FechaCreacion") 
            .IsRequired(); 
 
        builder.Property(p => p.RolRequiereValidacion)
            .HasColumnName("RolRequiereValidacion") 
            .IsRequired(); 
 
        builder.Property(p => p.RolAsignadoValidado)
            .HasColumnName("RolAsignadoValidado"); 
 
        builder.Property(p => p.PoliticaAsignadaBase)
            .HasColumnName("PoliticaAsignadaBase") 
            .IsRequired(); 
    }
}

