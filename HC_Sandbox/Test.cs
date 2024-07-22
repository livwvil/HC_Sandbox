namespace HC_Sandbox;

[Node]
[ExtendObjectType<Toyota>(IgnoreFields = [nameof(Toyota.Some)])]
public class ToyotaExtensions
{
    [BindMember(nameof(Toyota.InsteadOfIdField))]
    public Guid GetId([Parent] Toyota parent)
    {
        return parent.InsteadOfIdField;
    }

    [NodeResolver]
    public static async Task<Toyota> GetAsync([ID] Guid id, EFDbContext context) => context.Toyotas.Find(id) ?? throw new ArgumentException("Not found");
}

[Node]
[ExtendObjectType<Nissan>(IgnoreFields = [nameof(Nissan.Some)])]
public class NissanExtensions
{
    [BindMember(nameof(Nissan.InsteadOfIdField))]
    public Guid GetId([Parent] Nissan parent)
    {
        return parent.InsteadOfIdField;
    }

    [NodeResolver]
    public static async Task<Nissan> GetAsync([ID] Guid id, EFDbContext context) => context.Nissans.Find(id) ?? throw new ArgumentException("Not found");
}

public class III : InterfaceType<Car>
{
    protected override void Configure(IInterfaceTypeDescriptor<Car> descriptor)
    {
        descriptor.Name("ICar");
    }
}

//public class UnionValue : UnionType
//{
//    protected override void Configure(IUnionTypeDescriptor descriptor)
//    {
//        descriptor.Name("UnionValue");

//        descriptor.Type<Toyota>();

//        descriptor.Type<Nissan>();
//    }
//}

[QueryType]
public class TestQueries
{
    // Just to get all ids
    public async Task<IEnumerable<Car>> GetAllSomesAsync(EFDbContext context)
    {
        return Enumerable.Concat<Car>(context.Toyotas, context.Nissans);
    }
}


//[MutationType]
//public class TestMutations
//{

//}