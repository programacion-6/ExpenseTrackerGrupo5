using Microsoft.EntityFrameworkCore;

public class BaseContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Expense> Expenses { get; set; }
    public DbSet<Income> Incomes { get; set; }
    public DbSet<Budget> Budgets { get; set; }
    public DbSet<Goal> Goals { get; set; }

    public BaseContext(DbContextOptions<BaseContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Agregar configuraciones adicionales para cada entidad
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

        // Configurar tipos de columna para decimal
        modelBuilder.Entity<Expense>().Property(e => e.Amount).HasColumnType("decimal(18,2)");
        modelBuilder.Entity<Income>().Property(i => i.Amount).HasColumnType("decimal(18,2)");
        modelBuilder.Entity<Budget>().Property(b => b.Amount).HasColumnType("decimal(18,2)");
        modelBuilder.Entity<Budget>().Property(b => b.CurrentAmount).HasColumnType("decimal(18,2)");
        modelBuilder.Entity<Goal>().Property(g => g.GoalAmount).HasColumnType("decimal(18,2)");
        modelBuilder.Entity<Goal>().Property(g => g.CurrentAmount).HasColumnType("decimal(18,2)");
    }
}
