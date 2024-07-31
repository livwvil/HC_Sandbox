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

[QueryType]
public class TestQueries
{
    [UseProjection]
    public IEnumerable<Device> AllDevices(EFDbContext dbContext)
    {
        return dbContext.Devices;
    }
}
