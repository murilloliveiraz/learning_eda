namespace BuildingBlocks.Contracts;

public record PaymentCreated(
    Guid PaymentId,
    Guid AccountId,
    decimal Amount,
    DateTime CreatedAt
);
