using System;
using System.Diagnostics;
using System.Threading;

class ProcessMonitor
{
    static void Main(string[] args)
    {
        if (args.Length < 3)
        {
            Console.WriteLine("Please provide the command line arguments.");
            return;
        }

        string processName = args[0];
        int maxtime = int.Parse(args[1]) * 60 * 1000;
        int monitorfre = int.Parse(args[2]) * 60 * 1000; 

        Timer timer = new Timer((state) =>
        {
            var processes = Process.GetProcessesByName(processName);
            if (processes.Length == 0)
            {
                Console.WriteLine("No process found.");
                return;
            }

            DateTime now = DateTime.Now;
            foreach (var process in processes)
            {
                if (process.StartTime.AddMilliseconds(maxtime) < now)
                {
                    process.Kill();
                    Console.WriteLine("Process killed. Name: {0}, ID: {1}, Start Time: {2}", process.ProcessName, process.Id, process.StartTime);
                }
            }
        }, null, 0, monitorfre);

       
        Console.WriteLine("Press 'Q' to stop the program.");
        while (Console.ReadKey().Key != ConsoleKey.Q) { }
        timer.Dispose();
    }
}
