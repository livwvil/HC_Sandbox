namespace Api;

[Node]
public class Person(Guid id)
{
    [ID]
    public Guid Id { get; set; } = id;
    public List<Guid> FriendIds { get; set; } = [];
    public int Mood { get; set; } = 0;

    public async Task<List<Person>> GetFriendsAsync(PersonByIdDataLoader dataLoader) => [.. (await dataLoader.LoadAsync(FriendIds))];

    [DataLoader]
    internal static async Task<IReadOnlyDictionary<Guid, Person>> GetPersonByIdAsync(IReadOnlyList<Guid> ids) =>
        (await PersonRepo.Get(ids)).ToDictionary(x => x.Id);
}

public class PersonRepo
{
    private static Random _rnd = new();

    public static async Task<List<Person>> Get(IReadOnlyList<Guid>? ids = null)
    {
        Console.WriteLine($"PersonRepo.Get({(ids is not null ? string.Join(", ", ids) : "")})");

        // The database returns the same entities, but with a different mood field value, which changes depending on some external factors.
        var p1 = new Person(new("00000000-0000-0000-0000-000000000001")) { Mood = _rnd.Next(0, 100) };
        var p2 = new Person(new("00000000-0000-0000-0000-000000000002")) { Mood = _rnd.Next(0, 100) };
        var p3 = new Person(new("00000000-0000-0000-0000-000000000003")) { Mood = _rnd.Next(0, 100) };
        var p4 = new Person(new("00000000-0000-0000-0000-000000000004")) { Mood = _rnd.Next(0, 100) };
        var p5 = new Person(new("00000000-0000-0000-0000-000000000005")) { Mood = _rnd.Next(0, 100) };
        var p6 = new Person(new("00000000-0000-0000-0000-000000000006")) { Mood = _rnd.Next(0, 100) };

        p1.FriendIds.Add(p2.Id);
        p1.FriendIds.Add(p3.Id);
        p1.FriendIds.Add(p5.Id);

        p4.FriendIds.Add(p2.Id);
        p4.FriendIds.Add(p3.Id);

        p2.FriendIds.Add(p1.Id);
        p2.FriendIds.Add(p4.Id);

        p3.FriendIds.Add(p1.Id);
        p3.FriendIds.Add(p4.Id);

        p5.FriendIds.Add(p1.Id);
        p5.FriendIds.Add(p6.Id);

        p6.FriendIds.Add(p5.Id);

        List<Person> repo = [p1, p2, p3, p4, p5, p6];


        // Simulating a long database query
        if (ids is not null)
        {
            var res = new List<Person>();
            foreach (var id in ids)
            {
                await Task.Delay(_rnd.Next(500, 1000));
                var it = repo.FirstOrDefault(x => x.Id == id);
                if (it != null)
                {
                    res.Add(it);
                }
            }
            return res;
        }

        return repo;
    }
}

[QueryType]
public class TestQueries
{
    public async Task<List<Person>> GetPersonsAsync() => await PersonRepo.Get();
    public async Task<Person> GetPersonByIdWithDataLoaderAsync([ID] Guid id, PersonByIdDataLoader dataLoader) => await dataLoader.LoadAsync(id);
    public async Task<IEnumerable<Person>> GetPersonByIdsWithDataLoaderAsync([ID] Guid[] id, PersonByIdDataLoader dataLoader) => await dataLoader.LoadAsync(id);
    public async Task<Person> GetPersonByIdWithRepoAsync([ID] Guid id) => (await PersonRepo.Get([id])).FirstOrDefault()!;
}


[MutationType]
public class TestMutations
{
    public async Task<Person> MutPersonByIdWithDataLoaderAsync([ID] Guid id, PersonByIdDataLoader dataLoader)
    {
        return await dataLoader.LoadAsync(id);
    }

    public async Task<Person> MutPersonByIdWithRepoAsync([ID] Guid id)
    {
        return (await PersonRepo.Get([id])).FirstOrDefault()!;
    }



    public async Task<IEnumerable<Person>> MutPersonByIdsWithDataLoaderAsync([ID] Guid[] ids, PersonByIdDataLoader dataLoader)
    {
        return await dataLoader.LoadAsync(ids);
    }

    public async Task<IEnumerable<Person>> MutPersonByIdsWithRepoAsync([ID] Guid[] ids)
    {
        return await PersonRepo.Get(ids);
    }
}


