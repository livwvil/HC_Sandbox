using Microsoft.EntityFrameworkCore;

namespace HC_Sandbox;

[Node]
[ExtendObjectType<Person>]
public class PersonNode
{
    [NodeResolver]
    public static async Task<Person> GetAsync(
        [ID] Guid id,
        PersonByIdDataLoader personById,
        CancellationToken cancellationToken
    ) => await personById.LoadAsync(id, cancellationToken);

    [DataLoader]
    internal static async Task<IReadOnlyDictionary<Guid, Person>> GetPersonByIdAsync(
        IReadOnlyList<Guid> ids,
        [Service] EFDbContext dbContext,
        CancellationToken token)
    {
        return await dbContext.Persons.Where(x => ids.Contains(x.Id)).ToDictionaryAsync(x => x.Id, EFDbContext.UnProxy, token);
    }

    [DataLoader]
    internal static async Task<ILookup<Guid, Car>> GetCarsByPersonIdAsync(
        IReadOnlyList<Guid> personIds,
        [Service] EFDbContext dbContext,
        CancellationToken token)
    {
        return dbContext.Cars.Where(x => personIds.Contains(x.PersonId)).ToLookup(x => x.PersonId);
    }

    public async Task<IEnumerable<Car>> GetCarAsync(
        [Parent] Person person,
        IPersonByIdDataLoader personById,
        ICarsByPersonIdDataLoader carsByPersonId,
        CancellationToken token)

    {
        var p = await personById.LoadAsync(person.Id, token);
        var c = await carsByPersonId.LoadAsync(p.Id, token);
        return c;
    }
}

[QueryType]
public class TestQueries
{
    // Just to get all ids
    public async Task<IEnumerable<Person>> GetAllPersonsAsync(EFDbContext dbContext)
    {
        return dbContext.Persons;
    }
}


//[MutationType]
//public class TestMutations
//{

//}