using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using PoeLogsParser.Enums;
using PoeLogsParser.Models;
using PoeLogsParser.Models.Abstractions;

namespace PoeLogsParser.Parsers
{
    public class PlayerJoinedAreaParser : Parser
    {
        private readonly List<Regex> _regsClean = new List<Regex>()
        {
            new Regex(": ", RegexOptions.IgnoreCase),
            new Regex(" has joined the area.", RegexOptions.IgnoreCase)
        };

        private readonly Regex _regMatch = new Regex(".+ has joined the area.", RegexOptions.IgnoreCase);


        public override bool CanParse(string line)
        {
            return _regMatch.IsMatch(line);
        }

        public override ILogEntry Parse(string line, LogEntry entry)
        {
            return new PlayerJoinedAreaLogEntry(entry)
            {
                Types = new[] {LogEntryType.System, LogEntryType.Party, LogEntryType.JoinArea},
                Player = line
            };
        }

        public override string Clean(string line)
        {
            return _regsClean.Aggregate(line, (current, reg) => reg.Replace(current, ""));
        }
    }
}