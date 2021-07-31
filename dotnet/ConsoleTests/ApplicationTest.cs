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
            ILogService ls = new LogService("C:/Games/Path of Exile/logs/Client.txt");
            ls.NewTradeLogEntry += Ls_NewTradeLogEntry;
            
            Console.ReadKey();
        }

        private void Ls_NewTradeLogEntry(TradeLogEntry logEntry)
        {
            var g = 0;
        }
    }
}