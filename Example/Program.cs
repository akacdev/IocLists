using System;
using System.Linq;
using System.Threading.Tasks;
using IocLists;

namespace Example
{
    public static class Program
    {
        public const string ListName = "testing-list";

        public static async Task Main()
        {
            Console.WriteLine("Enter your IOCLists.com API key:");
            string key = Console.ReadLine();
            IocListsClient client = new(key);

            Console.WriteLine("Enter your IOCLists.com account username:");
            string username = Console.ReadLine();


            Console.WriteLine("\n> Creating a new list");
            await client.CreateList(username, ListName, "This is a list created using the C# IOC Lists library.");
            
            Console.WriteLine("List successfully created.");


            Console.WriteLine("\n> Adding an indicator");
            await client.Add(username, ListName, "https://example[.]com -- Testing Indicator");

            Console.WriteLine("Indicator successfully created. It may take a few seconds for the indicator to appear.");


            Console.WriteLine("\n> Getting the most recent entries from 'mirrors/phishtank'");
            Entry[] recent = await client.GetRecent("mirrors", "phishtank");
            
            Console.WriteLine($"Fetched the following entries:");
            foreach (Entry entry in recent) Console.WriteLine($"({entry.AddedAt}) {entry.Raw}");


            Console.WriteLine("\n> Getting all unique entries from 'mirrors/phishtank'");
            string[] unique = await client.GetUnique("mirrors", "phishtank");

            Console.WriteLine($"Fetched {unique.Length} unique entries");
            Console.WriteLine($"Preview: {string.Join(", ", unique.Take(5))}");


            Console.WriteLine("\n> Searching for an indicator across the platform");
            Entry[] matches = await client.Search("62.216.168.7");

            Console.WriteLine($"Query is present in {matches.Length} indicator{(matches.Length == 1 ? "" : "s")}.");
            Console.WriteLine($"Lists: {string.Join(", ", matches.Select(entry => $"{entry.Username}/{entry.ListName}"))}");


            Console.WriteLine("\nDemo finished.");
            Console.ReadKey();
        }
    }
}