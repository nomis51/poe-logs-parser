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

            ls.Parse("2021/01/23 13:31:37 321298453 bb2 [INFO Client 46012] @From dogmatixx: Hi, I would like to buy your The Dream Trial listed for 1 chaos in Expedition (stash tab \"$\"; position: left 12, top 7)");
            
            Console.ReadKey();
        }

        private void Ls_NewTradeLogEntry(TradeLogEntry logEntry)
        {
            var g = 0;
        }
    }
}