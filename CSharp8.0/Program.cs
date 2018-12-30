using System.Threading.Tasks;
using System.Collections.Generic;
using static System.Console;

class Program
{
    static void Main(string[] args)
    {
        var subscribers = Service.GetSubscribers();
        var names = GetNames(subscribers);

        foreach (var name in names)
        {
            WriteLine($"{name} has subscribed to the service");
        }

        ReadKey(true);
    }

    static IEnumerable<string> GetNames(IEnumerable<Person> people)
    {
        foreach (var p in people)
        {
            yield return GetName(p);
        }
    }

    static string GetName(Person p)
    {
        return $"{p.FirstName} {p.MiddleName?[0]} {p.LastName}";
    }
}

class Person
{
    public string FirstName { get; set; }
    public string MiddleName { get; set; }
    public string LastName { get; set; }

    public Person(string first, string last)
    {
        FirstName = first;
        LastName = last;
    }
}

class Service
{
    private static Person[] people =
    {
        new Person("Dustin", "Campbell"),
        new Person("Miguel", "de Icaza")
    };

    public static IEnumerable<Person> GetSubscribers()
    {
        foreach (var p in people) yield return p;
    }

    //public static async IAsyncEnumerable<Person> GetSubscribersAsync()
    //{
    //    for (int i = 0; i < people.Length; i++)
    //    {
    //        await Task.Delay(100);
    //        yield return people[i];
    //    }
    //}
}

