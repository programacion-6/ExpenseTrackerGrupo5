using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

public class BaseContextFactory : IDesignTimeDbContextFactory<BaseContext>
{
    public BaseContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<BaseContext>();
        optionsBuilder.UseNpgsql("Host=localhost;Database=expense_tracker_group_5_postgres;Username=user;Password=password");

        return new BaseContext(optionsBuilder.Options);
    }
}
