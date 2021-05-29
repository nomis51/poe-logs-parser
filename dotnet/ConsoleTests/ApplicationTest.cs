using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using PoeLogsParser.Models;
using PoeLogsParser.Models.Abstractions;
using PoeLogsParser.Parsers;
using PoeLogsParser.Services;
using PoeLogsParser.Services.Abstractions;

namespace ConsoleTests
{
    public class ApplicationTest
    {
        public void Run()
        {
            ILogService ls = new LogService("D:/Games/Path of Exile/logs/Client.txt");
            ls.NewChatMessageLogEntry += delegate(ChatMessageLogEntry entry)
            {
                var t = 0; // Handle the log entry
            };
            
            
            Console.ReadKey();
        }
    }
}