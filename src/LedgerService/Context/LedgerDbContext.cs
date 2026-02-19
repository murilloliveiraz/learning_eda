using Building_Blocks.Contracts;
using Microsoft.EntityFrameworkCore;

namespace LedgerService.Context;

public class LedgerDbContext : DbContext
{
    public LedgerDbContext(DbContextOptions<LedgerDbContext> options)
        : base(options)
    {
    }
    public DbSet<LedgerEntry> LedgerEntries { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<LedgerEntry>()
            .HasIndex(x => x.OperationId)
            .IsUnique();
    }
}
