using System.Threading.Tasks;
using System.Collections.Generic;
using static System.Console;
#nullable enable

class Program
{
    static async Task Main()
    {
        var subscribers = Service.GetSubscribersAsync();
        var names = GetNames(subscribers);

        await foreach (var name in names)
        {
            WriteLine($"{name} has subscribed to the service");
        }

        ReadKey(true);
    }

    static async IAsyncEnumerable<string> GetNames(IAsyncEnumerable<Person> people)
    {
        await foreach (var p in people)
        {
            yield return GetName(p);
        }
    }

    //static string GetName(Person p)
    //{
    //    return (p.MiddleName is null) 
    //        ? $"{p.FirstName} {p.LastName}"
    //        : $"{p.FirstName} {p.MiddleName[0]} {p.LastName}";
    //}

    static string GetName(Person p)
    {
        return (p.FirstName, p.MiddleName, p.LastName) switch
        {
            (string f, string m, string l) => $"{f} {m[0]}. {1}",
            (string f, null    , string l) => $"{f} {l}",
            (string f, string m, null    ) => $"{f} {m}",
            (string f, null    , null    ) => f,
            (null    , string m, string l) => $"Ms/Mr {m[0]}. {1}",
            (null    , null    , string l) => $"Ms/Mr {1}",
            (null    , string m, null    ) => $"Ms/Mr {m}",
            (null    , null    , null    ) => "Someone"
        };
    }
}

class Person
{
    public string FirstName { get; set; }
    public string? MiddleName { get; set; }
    public string LastName { get; set; }

    public Person(string first, string last)
    {
        FirstName = first;
        MiddleName = null;
        LastName = last;
    }

    public Person(string first, string middlename, string last)
    {
        FirstName = first;
        MiddleName = middlename;
        LastName = last;
    }
}

class Service
{
    private static readonly Person[] people =
    {
        new Person("Carl", "Scott", "Campbell"),
        new Person("Neal", "M", "Campbell"),
        new Person("Dustin", "Campbell"),
        new Person("Miguel", "de Icaza")
    };

    // Still does not work in the fisrt preview 2.0
    //private static Person[] people =
    //{
    //    new ("Carl", "Scott", "Campbell"),
    //    new ("Neal", "M", "Campbell"),
    //    new ("Dustin", "Campbell"),
    //    new ("Miguel", "de Icaza")
    //};

    public static IEnumerable<Person> GetSubscribers()
    {
        foreach (var p in people[1..^1]) yield return p;
    }

    public static async IAsyncEnumerable<Person> GetSubscribersAsync()
    {
        for (int i = 0; i < people.Length; i++)
        {
            await Task.Delay(100);
            yield return people[i];
        }
    }
}

interface IILooger
{
    void Log(LogLevel level, string message);
    // Still does not work in the fisrt preview 2.0
    //void Log(System.Exception ex) => Log(LogLevel.Error, ex.ToString);
}

class ConsoleLogger : IILooger
{
    public void Log(LogLevel level, string message) { }
}

class LogLevel
{
    const string Error = nameof(Error);
}
