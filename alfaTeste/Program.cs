using alfaTeste;

var path = args.Length > 0 ? args[0]: null;
while (string.IsNullOrEmpty(path))
{
    Console.WriteLine("Enter the path to the file");
    path = Console.ReadLine();
}
var logIO = new LogIO(path, "logs.txt");

if (!logIO.IsRunnable)
{
    Console.WriteLine("wait 1 minute to run this application again");
    Thread.Sleep(5000);
    Environment.Exit(0);
}
await logIO.GetUserInformation();

Console.WriteLine("finished");
Thread.Sleep(5000);
