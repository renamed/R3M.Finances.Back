using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WebApi.Model;

namespace WebApi.Context;

public class FinancesContext(DbContextOptions<FinancesContext> dbContextOptions) : DbContext(dbContextOptions)
{
    public DbSet<Category>  Categories { get; set; }
    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<Period> Periods { get; set; }
    public DbSet<FinancialGoal> FinancialGoals { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ConfigCategories(modelBuilder);
        ConfigTransactions(modelBuilder);
        ConfigPeriods(modelBuilder);
        ConfigFinancialGoals(modelBuilder);

        base.OnModelCreating(modelBuilder);
    }

    private static void ConfigFinancialGoals(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<FinancialGoal>(e =>
        {
            ConfigId(e);

            e.Property(p => p.CategoryId)
                .IsRequired();
            e.Property(p => p.PeriodId)
                .IsRequired();

            e.HasIndex(i => new { i.CategoryId, i.PeriodId }).IsUnique();

            e.HasOne(o => o.Category).WithMany().HasForeignKey(o => o.CategoryId);
            e.HasOne(o => o.Period).WithMany().HasForeignKey(o => o.PeriodId);
        });
    }

    private static void ConfigPeriods(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Period>(e =>
        {
            ConfigId(e);

            e.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(10);

            e.Property(e => e.Start).IsRequired();
            e.Property(e => e.End).IsRequired();

            e.HasIndex(i => new { i.Name, i.Start });
            e.HasIndex(i => i.Name).IsUnique();
        });
    }

    private static void ConfigTransactions(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Transaction>(e =>
        {
            ConfigId(e);

            e.Property(p => p.Description)
                .IsRequired()
                .HasMaxLength(250);

            e.HasIndex(i => i.InvoiceDate).IsDescending();

            e.HasOne(o => o.Category).WithMany().HasForeignKey(o => o.CategoryId).IsRequired();
            e.HasOne(o => o.Period).WithMany().HasForeignKey(o => o.PeriodId).IsRequired();
        });
    }

    private static void ConfigCategories(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(e =>
        {
            ConfigId(e);

            e.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(100);

            e.HasIndex(i => new { i.Name, i.TransactionType })
                .IsUnique();

            e.Property(p => p.TransactionType)
                .IsRequired()
                .HasConversion(new EnumToStringConverter<TransactionType>());

            e.Property(p => p.ParentId).IsRequired(false);

            e.HasOne(i => i.Parent).WithMany().HasForeignKey(p => p.ParentId).IsRequired(false);
        });
    }

    private static void ConfigId<T>(EntityTypeBuilder<T> e) where T : Register
    {
        e.HasKey(p => p.Id);
        e.Property(p => p.Id).ValueGeneratedOnAdd();
    }

    public override int SaveChanges()
    {
        SetMetaDate();
        return base.SaveChanges();
    }

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        SetMetaDate();
        return base.SaveChanges(acceptAllChangesOnSuccess);
    }

    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        SetMetaDate();
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        SetMetaDate();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void SetMetaDate()
    {
        var entities = ChangeTracker.Entries().Where(x => x.State == EntityState.Added || x.State == EntityState.Modified);

        foreach(var entity in entities)
        {
            if (entity.State == EntityState.Added)
            {
                ((Register)entity.Entity).Inserted = DateTime.UtcNow;
            }
            else if (entity.State == EntityState.Modified)
            {
                ((Register)entity.Entity).Updated = DateTime.UtcNow;
            }
        }
    }
}
