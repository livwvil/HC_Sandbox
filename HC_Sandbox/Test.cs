using HotChocolate.Types.Descriptors;
using System.Reflection;

namespace HC_Sandbox;

public class MyTypeInspector : DefaultTypeInspector
{
    public override bool IsMemberIgnored(MemberInfo member)
    {
        if (member is null)
        {
            throw new ArgumentNullException(nameof(member));
        }

        //if (member.Name == "Some")
        //{
        //    return true;
        //}
        var a = member as PropertyInfo;
        if (a != null && a.PropertyType.Name.Contains(nameof(ISome)))
        {
            return true;
        }

        return base.IsMemberIgnored(member);
    }

}

[ExtendObjectType<Toyota>(IgnoreProperties = [nameof(Toyota.Some)])]
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
[ExtendObjectType<Nissan>(IgnoreProperties = [nameof(Nissan.Some)])]
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