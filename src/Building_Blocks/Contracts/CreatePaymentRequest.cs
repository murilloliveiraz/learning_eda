namespace Building_Blocks.Contracts;

public record CreatePaymentRequest(
    Guid AccountId,
    decimal Amount
);
