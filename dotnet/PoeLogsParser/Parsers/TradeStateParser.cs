using System.Collections.Generic;
using System.Text.RegularExpressions;
using PoeLogsParser.Enums;
using PoeLogsParser.Models;
using PoeLogsParser.Models.Abstractions;

namespace PoeLogsParser.Parsers
{
    public class TradeStateParser : Parser
    {
        private readonly Regex _regMatch = new Regex("Trade (accepted|cancelled)", RegexOptions.IgnoreCase);

        public override bool CanParse(string line)
        {
            return _regMatch.IsMatch(line);
        }

        public override ILogEntry Parse(string line, LogEntry entry)
        {
            var isAccepted = line.Contains("Trade accepted");
            var isCancelled = line.Contains("Trade cancelled");

           return new TradeStateLogEntry(entry)
            {
                IsAccepted = isAccepted,
                IsCancelled = isCancelled,
                Types = new []
                {
                    LogEntryType.Trade,
                    LogEntryType.System,
                    LogEntryType.TradeState
                }
            };
        }

        public override string Clean(string line)
        {
            return line;
        }
    }
}