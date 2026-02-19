using Building_Blocks.Contracts;
using BuildingBlocks.Contracts;
using LedgerService.Context;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace LedgerService.Consumers;

public class PaymentCreatedConsumer : IConsumer<PaymentCreated>
{
    protected readonly LedgerDbContext db;

    public PaymentCreatedConsumer(LedgerDbContext _db)
    {
        db = _db;
    }
    public async Task Consume(ConsumeContext<PaymentCreated> context)
    {
        var msg = context.Message;

        Console.WriteLine($"Received payment {msg.PaymentId}");
        Console.WriteLine($"Retry attempt: {context.GetRetryAttempt()}");


        var entry = new LedgerEntry {
            OperationId = msg.PaymentId,
            AccountId =  msg.AccountId,
            Amount = msg.Amount
        };

        try
        {
            db.LedgerEntries.Add(entry);
            await db.SaveChangesAsync();
            //Environment.Exit(1);
        }
        catch (DbUpdateException ex)
        {
            // Already processed
            return;
        }
    }
}
