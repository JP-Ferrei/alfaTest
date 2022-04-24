using alfaTeste;

Console.WriteLine("Enter the path to the file");

var path = Console.ReadLine();
while (string.IsNullOrEmpty(path))
{
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
