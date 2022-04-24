using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace alfaTeste
{
    public class LogIO
    {
        public HashSet<string> Users { get; private set; }
        public string InputPath { get; set; }
        public string OutputPath { get; set; }
        public bool IsRunnable { get; private set; } = true;
        public string BaseUrl { get; set; } = "https://api.bitbucket.org/2.0/users";

        public LogIO(string inputPath, string outputPath)
        {
            InputPath = inputPath;
            OutputPath = outputPath;
            Users = new HashSet<string>();
            CanProgramRun();
        }

        public HashSet<string> GetUsers()
        {
            try
            {
                using StreamReader sr = File.OpenText(InputPath);
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    Users.Add(line);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return Users;
        }

        public async Task GetUserInformation()
        {
            if(Users.Count == 0)
            {
                GetUsers();
            }

            var client = new HttpClient();
            foreach (var user in Users)
            {
                var path = $"{BaseUrl}/{user}";
                var res = await client.GetAsync(path);
                var resString = await res.Content.ReadAsStringAsync();
                Console.WriteLine($"fetching user: {user}, url: {path}");
                await WriteLog(resString);
                await Task.Delay(5000);
            };
        }

        private async Task WriteLog(string log)
        {
            try
            {
                using var sw = File.AppendText(OutputPath);
                var logMessage = $"{DateTime.Now} {log}";
                await sw.WriteLineAsync(logMessage);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private int CanProgramRun()
        {
            var fileInfo = new FileInfo(OutputPath);
            var lastTime = fileInfo.LastWriteTime;
            var now = DateTime.Now;
            var min = now.Subtract(lastTime).Minutes;
            IsRunnable = min > 1;
            return min;
        }
    }
}
