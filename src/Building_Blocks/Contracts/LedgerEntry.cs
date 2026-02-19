namespace Building_Blocks.Contracts;
public class LedgerEntry
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public Guid OperationId { get; init; }
    public Guid AccountId { get; init; }
    public decimal Amount { get; init; }
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
}

