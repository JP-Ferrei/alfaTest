Console.WriteLine("Enter the path to the file");

var isRunnable = CanProgramRun();
if (!isRunnable)
{
    Console.WriteLine("wait 1 minute to run this application again");
    Thread.Sleep(5000);
    Environment.Exit(0);

}
var path = Console.ReadLine();

var users = GetUsers(path);
await GetUserInformation(users.ToList());
Console.WriteLine("finished");
Thread.Sleep(5000);

static HashSet<string> GetUsers(string path)
{
    var userList = new HashSet<string>();

    try
    {
        using StreamReader sr = File.OpenText(path);
        string line;
        while ((line = sr.ReadLine()) != null)
        {
            userList.Add(line);
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }
    return userList;
}

static async Task GetUserInformation(List<string> users)
{
    
    var client = new HttpClient();
    var baseUrl = "https://api.bitbucket.org/2.0/users";
    foreach (var user in users)
    {
        var path = $"{baseUrl}/{user}";
        var res = await client.GetAsync(path);
        var resString = await res.Content.ReadAsStringAsync();
        Console.WriteLine($"fetching user: {user}, url: {path}");
        await WriteLog(resString);
        await Task.Delay(5000);
    };
}

static async Task WriteLog(string log)
{
    try
    {
        using var sw = File.AppendText("logs.txt");
        var logMessage = $"{DateTime.Now} {log}";
        await sw.WriteLineAsync(logMessage);
        
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }
}

static bool CanProgramRun()
{
    var fileInfo = new FileInfo("logs.txt");
    var lastTime = fileInfo.LastWriteTime;
    var now = DateTime.Now;
    var min = now.Subtract(lastTime).Minutes;
    return min > 1;
}
