namespace AUT2Services.Infra.Security.Records;

public sealed record BuscarRolesResponse(
    IReadOnlyCollection<RolListaItemResponse> Items
);
