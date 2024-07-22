using Microsoft.EntityFrameworkCore;

namespace HC_Sandbox;

public interface ISome { }

public abstract class Car
{
    public string Name2 { get; }
    public ISome[] Some { get; } = [];
}

public class Toyota : Car
{
    public Toyota() { }

    public Guid InsteadOfIdField { get; set; }

    public string Name { get; set; }

    public Toyota(Guid id, string name)
    {
        InsteadOfIdField = id;
        Name = name;
    }
}

public class Nissan : Car
{
    public Nissan() { }

    public Guid InsteadOfIdField { get; set; }

    public string Name { get; set; }

    public Nissan(Guid id, string name)
    {
        InsteadOfIdField = id;
        Name = name;
    }
}

public class EFDbContext(DbContextOptions<EFDbContext> options) : DbContext(options)
{
    public DbSet<Toyota> Toyotas { get; set; }
    public DbSet<Nissan> Nissans { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Toyota>(x => x.HasKey(xx => xx.InsteadOfIdField));
        modelBuilder.Entity<Nissan>(x => x.HasKey(xx => xx.InsteadOfIdField));
    }

    public static void Populate(EFDbContext context)
    {
        context.Toyotas.Add(new Toyota(new("00000000-1000-0000-0000-000000000001"), "toyota1"));
        context.Nissans.Add(new Nissan(new("00000000-2000-0000-0000-000000000001"), "nissan1"));

        context.SaveChanges();
    }
}

