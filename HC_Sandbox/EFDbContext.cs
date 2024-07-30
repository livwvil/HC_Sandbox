using Microsoft.EntityFrameworkCore;

namespace HC_Sandbox;

public class DeviceName : ValueObject<DeviceName>
{
    public DeviceName() { }

    public DeviceName(string name)
    {
        Value = name;
    }
    
    public string Value { get; private set; }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}

public class Device
{
    public Device() { }

    public Device(Guid id, string name)
    {
        Id = id;
        Name = new(name);
    }
    
    public Guid Id { get; private set; }

    public DeviceName Name { get; private set; }
}

public class Trip
{
    public Trip() { }

    public Trip(Guid id, string name, Guid deviceId)
    {
        Id = id;
        Name = new(name);
        DeviceId = deviceId;
    }

    public Guid Id { get; private set; }

    public Guid DeviceId { get; private set; }

    public string Name { get; private set; }
}

public class EFDbContext(DbContextOptions<EFDbContext> options) : DbContext(options)
{
    public DbSet<Device> Devices { get; set; }
    public DbSet<Trip> Trips { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Device>()
            .OwnsOne(x => x.Name);
        modelBuilder.Entity<Device>()
            .HasMany<Trip>()
            .WithOne()
            .HasForeignKey(x => x.DeviceId);
    }

    public static void Populate(EFDbContext context)
    {
        var d1 = new Device(new("00000000-0000-0000-0000-000000000001"), "dev1");
        context.Devices.Add(d1);

        var t1 = new Trip(new("00000000-0000-0000-0000-100000000003"), "trip1", d1.Id);
        context.Trips.Add(t1);
        
        context.SaveChanges();
    }
}

public abstract class ValueObject<T> where T : ValueObject<T>
{
    protected abstract IEnumerable<object> GetEqualityComponents();

    public override bool Equals(object? obj)
    {
        if (obj is not T valueObject)
        {
            return false;
        }

        return EqualsCore(valueObject);
    }

    private bool EqualsCore(ValueObject<T> other)
    {
        return GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
    }

    public override int GetHashCode()
    {
        return GetEqualityComponents().Aggregate(1, (current, obj) => current * 17 + (obj?.GetHashCode() ?? 0));
    }

    public static bool operator ==(ValueObject<T> a, ValueObject<T> b)
    {
        if (a is null && b is null)
        {
            return true;
        }

        if (a is null || b is null)
        {
            return false;
        }

        return a.Equals(b);
    }

    public static bool operator !=(ValueObject<T> a, ValueObject<T> b)
    {
        return !(a == b);
    }
}