using AUT2Services.Infra.Security.Models;

namespace AUT2Services.Infra.Security.Records;

public sealed record SelectedSessionContext(
    string? CodigoOrganizacionSeleccionada,
    string? NombreOrganizacionSeleccionada,
    string? CodigoUnidadOrganizacionalSeleccionada,
    string? NombreUnidadOrganizacionalSeleccionada,
    EntidadRolSeleccionado? EntidadRolSeleccionado
);
