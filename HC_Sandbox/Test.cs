namespace HC_Sandbox;

public class DeviceType : ObjectType<Device>
{
    protected override void Configure(IObjectTypeDescriptor<Device> descriptor)
    {
        descriptor.BindFieldsExplicitly();
        descriptor.ImplementsNode();

        descriptor.Field(x => x.Id).ID();
        descriptor.Field(x => x.Name.Value).Name("name");
    }
}

public class TripType : ObjectType<Trip>
{
    protected override void Configure(IObjectTypeDescriptor<Trip> descriptor)
    {
        descriptor.BindFieldsExplicitly();

        descriptor.Field(x => x.Id).ID();
        descriptor.Field(x => x.Name);
    }
}

[Node]
public class TripSpecial
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public Device Device { get; set; }
}

[QueryType]
public class TestQueries
{
    [UsePaging(DefaultPageSize = int.MaxValue, MaxPageSize = int.MaxValue, IncludeTotalCount = true)]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IEnumerable<Device> AllDevices(EFDbContext dbContext)
    {
        return dbContext.Devices;
    }

    public IEnumerable<Trip> AllTrip(EFDbContext dbContext)
    {
        return dbContext.Trips;
    }

    [UsePaging(DefaultPageSize = int.MaxValue, MaxPageSize = int.MaxValue, IncludeTotalCount = true)]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IEnumerable<TripSpecial> AllTripSpecial(EFDbContext dbContext)
    {
        return dbContext.Trips.Join(dbContext.Devices, x => x.DeviceId, x => x.Id,
            (t, d) => new TripSpecial()
            {
                Id = t.Id,
                Name = t.Name,
                Device = d
            });
    }
}
