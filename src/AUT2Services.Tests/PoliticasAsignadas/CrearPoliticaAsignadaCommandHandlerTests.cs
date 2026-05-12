using AUT2Services.Domain.Commands.PoliticasAsignadas.Commands;
using AUT2Services.Domain.Commands.PoliticasAsignadas.Handlers;
using AUT2Services.Domain.Core.Auditing;
using AUT2Services.Domain.Core.Data;
using AUT2Services.Domain.Entities;
using AUT2Services.Domain.Interfaces;
using AUT2Services.Tests.Support;

namespace AUT2Services.Tests.PoliticasAsignadas;

[TestClass]
public class CrearPoliticaAsignadaCommandHandlerTests
{
    [TestMethod]
    public async Task Handle_sets_creation_dates_from_clock_before_persisting()
    {
        var utcNow = new DateTimeOffset(2026, 5, 12, 10, 30, 0, TimeSpan.Zero);
        var repository = new CapturingPoliticaAsignadaRepository();
        var handler = new PoliticaAsignadaCommandHandler(
            repository,
            new TestClock(utcNow),
            NoOpAuditBuffer.Instance);

        var command = new CrearPoliticaAsignadaCommand(
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            rolRequiereValidacion: true);

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.IsTrue(result.Result);
        Assert.IsNotNull(repository.Created);
        Assert.AreEqual(utcNow, repository.Created.FechaCreacion);
        Assert.AreEqual(utcNow, repository.Created.FechaInicioAsignacion);
        Assert.IsNull(repository.Created.FechaTerminoAsignacion);
        Assert.IsTrue(repository.Created.RolRequiereValidacion);
        Assert.IsFalse(repository.Created.RolAsignadoValidado);
        Assert.IsFalse(repository.Created.PoliticaAsignadaBase);
    }

    private sealed class CapturingPoliticaAsignadaRepository : IPoliticaAsignadaRepository
    {
        public PoliticaAsignada? Created { get; private set; }

        public IUnitOfWork UnitOfWork { get; } = new SuccessfulUnitOfWork();

        public void Crear(PoliticaAsignada data)
        {
            Created = data;
        }

        public void Modificar(PoliticaAsignada data)
        {
            throw new NotSupportedException();
        }

        public void Eliminar(PoliticaAsignada data)
        {
            throw new NotSupportedException();
        }

        public Task<PoliticaAsignada> BuscarPor_Id(Guid id)
        {
            return Task.FromResult<PoliticaAsignada>(null!);
        }

        public Task<IEnumerable<PoliticaAsignada>> BuscarPor_Id_Entidad_Id_Rol_Id_Proceso(
            Guid id_Entidad,
            Guid id_Rol,
            Guid id_Proceso)
        {
            return Task.FromResult<IEnumerable<PoliticaAsignada>>([]);
        }

        public Task<IEnumerable<PoliticaAsignada>> BuscarPor_Id_Entidad(Guid id_Entidad)
        {
            return Task.FromResult<IEnumerable<PoliticaAsignada>>([]);
        }

        public Task<IEnumerable<PoliticaAsignada>> BuscarPor_Id_Entidades(IEnumerable<Guid> ids_Entidad)
        {
            return Task.FromResult<IEnumerable<PoliticaAsignada>>([]);
        }

        public Task<IEnumerable<PoliticaAsignada>> BuscarPor_Id_Rol(Guid id_Rol)
        {
            return Task.FromResult<IEnumerable<PoliticaAsignada>>([]);
        }

        public Task<IEnumerable<PoliticaAsignada>> BuscarPor_Id_Proceso(Guid id_Proceso)
        {
            return Task.FromResult<IEnumerable<PoliticaAsignada>>([]);
        }

        public Task<IEnumerable<PoliticaAsignada>> BuscarPor_RolRequiereValidacion()
        {
            return Task.FromResult<IEnumerable<PoliticaAsignada>>([]);
        }

        public void Dispose()
        {
        }
    }

    private sealed class SuccessfulUnitOfWork : IUnitOfWork
    {
        public Task<bool> Commit()
        {
            return Task.FromResult(true);
        }
    }
}
