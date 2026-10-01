using AUT2Services.Domain.Core.Auditing;
using AUT2Services.Infra.DataMongoDB.Models;
using AUT2Services.Infra.DataTrazabilidad.Auditing;
using AUT2Services.Infra.Security.Traceability;

namespace AUT2Services.Tests.Auditing;

[TestClass]
public sealed class AuditSensitiveDataRedactionTests
{
    private readonly SystemTextJsonAuditSerializer serializer = new();

    [TestMethod]
    public void Build_WhenTokenChanges_RetainsThePathAndRedactsTheValue()
    {
        var builder = new DefaultAuditDeltaBuilder(serializer);

        var changes = builder.Build(
            new AuditSample { Token = "previous-test-value", MaximaAsignacionDeRoles = 1 },
            new AuditSample { Token = "current-test-value", MaximaAsignacionDeRoles = 2 });

        var tokenChange = changes.Single(change => change.Path == nameof(AuditSample.Token));
        var maximaChange = changes.Single(change => change.Path == nameof(AuditSample.MaximaAsignacionDeRoles));

        Assert.AreEqual(AuditSensitiveDataPolicy.RedactedJsonValue, tokenChange.NewValueJson);
        Assert.IsFalse(tokenChange.NewValueJson!.Contains("current-test-value", StringComparison.Ordinal));
        Assert.AreEqual("2", maximaChange.NewValueJson);
    }

    [TestMethod]
    public void Serialize_WhenSnapshotContainsSensitiveFields_OmitsThemAndKeepsNonSensitiveFields()
    {
        var snapshot = serializer.Serialize(new AuditSample
        {
            Nombre = "Proceso visible",
            Token = "snapshot-test-value",
            MaximaAsignacionDeRoles = 2
        }) ?? throw new InvalidOperationException("El serializer no produjo un snapshot.");

        Assert.IsFalse(snapshot.Contains(nameof(AuditSample.Token), StringComparison.Ordinal));
        Assert.IsFalse(snapshot.Contains("snapshot-test-value", StringComparison.Ordinal));
        Assert.IsTrue(snapshot.Contains(nameof(AuditSample.Nombre), StringComparison.Ordinal));
        Assert.IsTrue(snapshot.Contains("Proceso visible", StringComparison.Ordinal));
    }

    [TestMethod]
    public void Serialize_WhenSecurityStateContainsValidationValues_OmitsThem()
    {
        var snapshot = serializer.Serialize(new UsuarioTraceabilityState
        {
            UserId = Guid.NewGuid().ToString(),
            ActionContext = "PASSWORD_CHANGE",
            ValidationToken = "validation-token-test-value",
            ValidationCode = "validation-code-test-value"
        }) ?? throw new InvalidOperationException("El serializer no produjo un snapshot.");

        Assert.IsFalse(snapshot.Contains(nameof(UsuarioTraceabilityState.ValidationToken), StringComparison.Ordinal));
        Assert.IsFalse(snapshot.Contains(nameof(UsuarioTraceabilityState.ValidationCode), StringComparison.Ordinal));
        Assert.IsFalse(snapshot.Contains("validation-token-test-value", StringComparison.Ordinal));
        Assert.IsFalse(snapshot.Contains("validation-code-test-value", StringComparison.Ordinal));
        Assert.IsTrue(snapshot.Contains(nameof(UsuarioTraceabilityState.ActionContext), StringComparison.Ordinal));
    }

    [TestMethod]
    public void Build_WhenSanitizedChangeIsMappedForMongo_DoesNotContainTheOriginalValue()
    {
        var builder = new DefaultAuditDeltaBuilder(serializer);
        var changes = builder.Build(
            new AuditSample { Token = "previous-test-value" },
            new AuditSample { Token = "mongo-test-value" });

        var document = new MongoAuditTimelineDocument
        {
            Id = Guid.NewGuid(),
            CorrelationId = Guid.NewGuid(),
            AggregateId = Guid.NewGuid(),
            AggregateType = "AuditSample",
            Changes = changes.Select(change => new MongoAuditChangeDocument
            {
                Order = change.Order,
                Path = change.Path,
                ValueType = change.ValueType,
                NewValueJson = change.NewValueJson
            }).ToArray()
        };

        var serializedDocument = serializer.Serialize(document) ?? throw new InvalidOperationException("El serializer no produjo un documento.");

        Assert.IsFalse(serializedDocument.Contains("mongo-test-value", StringComparison.Ordinal));
        Assert.IsTrue(serializedDocument.Contains("[REDACTED]", StringComparison.Ordinal));
    }

    private sealed class AuditSample
    {
        public string? Nombre { get; init; }
        public string? Token { get; init; }
        public int MaximaAsignacionDeRoles { get; init; }
    }
}
