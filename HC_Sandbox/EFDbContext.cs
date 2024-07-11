using Microsoft.EntityFrameworkCore;

namespace HC_Sandbox;

public class Person
{
    public Person() { }

    public Person(Guid id, string name)
    {
        Id = id;
        Name = name;
    }
    
    public Guid Id { get; set; }

    public string Name { get; set; }
}

public class Car
{
    public Car() { }

    public Car(Guid id, string name, Guid personId)
    {
        Id = id;
        Name = name;
        PersonId = personId;
    }

    public Guid Id { get; set; }

    public string Name { get; set; }

    public Guid PersonId { get; set; }

    public virtual List<CarPart> CarsParts { get; set; }
}

public class CarPart
{
    public CarPart() { }

    public CarPart(Guid id, string name, Guid carId)
    {
        Id = id;
        Name = name;
        CarId = carId;
    }

    public Guid Id { get; set; }

    public Guid CarId { get; set; }

    public string Name { get; set; }
}

public class EFDbContext(DbContextOptions<EFDbContext> options) : DbContext(options)
{
    public DbSet<Person> Persons { get; set; }
    public DbSet<Car> Cars { get; set; }
    public DbSet<CarPart> CarParts { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Person>()
            .HasMany<Car>()
            .WithOne()
            .HasForeignKey(x => x.PersonId);

        modelBuilder.Entity<Car>()
            .HasMany<CarPart>()
            .WithOne()
            .HasForeignKey(x => x.CarId);
    }

    public static void Populate(EFDbContext context)
    {
        var p1 = new Person(new("00000000-0000-0000-0000-000000000001"), "Petya");
        var p2 = new Person(new("00000000-0000-0000-0000-000000000002"), "Kolya");
        var p3 = new Person(new("00000000-0000-0000-0000-000000000003"), "Vasya");
        var p4 = new Person(new("00000000-0000-0000-0000-000000000004"), "Dima");
        context.Persons.Add(p1);
        context.Persons.Add(p2);
        context.Persons.Add(p3);
        context.Persons.Add(p4);


        var c1 = new Car(new("00000000-0000-0000-0000-100000000001"), "toyota mark 1", p1.Id);
        var c2 = new Car(new("00000000-0000-0000-0000-100000000002"), "toyota mark 2", p1.Id);
        var c3 = new Car(new("00000000-0000-0000-0000-100000000003"), "toyota caldina", p2.Id);
        var c4 = new Car(new("00000000-0000-0000-0000-100000000004"), "toyota carina", p3.Id);
        var c5 = new Car(new("00000000-0000-0000-0000-100000000005"), "toyota levin", p4.Id);
        context.Cars.Add(c1);
        context.Cars.Add(c2);
        context.Cars.Add(c3);
        context.Cars.Add(c4);
        context.Cars.Add(c5);

        var cp1_1 = new CarPart(new("00000000-0000-0000-0000-210000000001"), "wheel1", c1.Id);
        var cp1_2 = new CarPart(new("00000000-0000-0000-0000-210000000002"), "wheel2", c1.Id);
        var cp1_3 = new CarPart(new("00000000-0000-0000-0000-210000000003"), "wheel3", c1.Id);
        var cp1_4 = new CarPart(new("00000000-0000-0000-0000-210000000004"), "wheel4", c1.Id);
        var cp2_1 = new CarPart(new("00000000-0000-0000-0000-220000000001"), "wheel1", c2.Id);
        var cp2_2 = new CarPart(new("00000000-0000-0000-0000-220000000002"), "wheel2", c2.Id);
        var cp2_3 = new CarPart(new("00000000-0000-0000-0000-220000000003"), "wheel3", c2.Id);
        var cp2_4 = new CarPart(new("00000000-0000-0000-0000-220000000004"), "wheel4", c2.Id);
        var cp3_1 = new CarPart(new("00000000-0000-0000-0000-230000000001"), "wheel1", c3.Id);
        var cp3_2 = new CarPart(new("00000000-0000-0000-0000-230000000002"), "wheel2", c3.Id);
        var cp3_3 = new CarPart(new("00000000-0000-0000-0000-230000000003"), "wheel3", c3.Id);
        var cp3_4 = new CarPart(new("00000000-0000-0000-0000-230000000004"), "wheel4", c3.Id);
        var cp4_1 = new CarPart(new("00000000-0000-0000-0000-240000000001"), "wheel1", c4.Id);
        var cp4_2 = new CarPart(new("00000000-0000-0000-0000-240000000002"), "wheel2", c4.Id);
        var cp4_3 = new CarPart(new("00000000-0000-0000-0000-240000000003"), "wheel3", c4.Id);
        var cp4_4 = new CarPart(new("00000000-0000-0000-0000-240000000004"), "wheel4", c4.Id);
        var cp5_1 = new CarPart(new("00000000-0000-0000-0000-250000000001"), "wheel1", c5.Id);
        var cp5_2 = new CarPart(new("00000000-0000-0000-0000-250000000002"), "wheel2", c5.Id);
        var cp5_3 = new CarPart(new("00000000-0000-0000-0000-250000000003"), "wheel3", c5.Id);
        var cp5_4 = new CarPart(new("00000000-0000-0000-0000-250000000004"), "wheel4", c5.Id);
        context.CarParts.Add(cp1_1);
        context.CarParts.Add(cp1_2);
        context.CarParts.Add(cp1_3);
        context.CarParts.Add(cp1_4);
        context.CarParts.Add(cp2_1);
        context.CarParts.Add(cp2_2);
        context.CarParts.Add(cp2_3);
        context.CarParts.Add(cp2_4);
        context.CarParts.Add(cp3_1);
        context.CarParts.Add(cp3_2);
        context.CarParts.Add(cp3_3);
        context.CarParts.Add(cp3_4);
        context.CarParts.Add(cp4_1);
        context.CarParts.Add(cp4_2);
        context.CarParts.Add(cp4_3);
        context.CarParts.Add(cp4_4);
        context.CarParts.Add(cp5_1);
        context.CarParts.Add(cp5_2);
        context.CarParts.Add(cp5_3);
        context.CarParts.Add(cp5_4);

        context.SaveChanges();
    }

    public static T UnProxy<T>(T efObject) where T : new()
    {
        var type = efObject.GetType();

        if (type.Namespace == "Castle.Proxies")
        {
            var baseType = type.BaseType;
            var returnObject = new T();
            foreach (var property in baseType.GetProperties())
            {
                var propertyType = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
                if (propertyType.Namespace == "System")
                {
                    var value = property.GetValue(efObject);
                    property.SetValue(returnObject, value);
                }
            }
            return returnObject;
        }

        return efObject;
    }
}

