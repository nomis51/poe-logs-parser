using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using PoeLogsParser.Models.Abstractions;
using PoeLogsParser.Services;
using PoeLogsParser.Services.Abstractions;

namespace ConsoleTests
{
    public class ApplicationTest
    {
        public void Run()
        {
            ILogService ls = new LogService("./Client.txt");
            ls.NewLogEntry += delegate(ILogEntry entry)
            {
                var t = 0; // Handle the log entry
            };

            Console.ReadKey();
        }
    }
}