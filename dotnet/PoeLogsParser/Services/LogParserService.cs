using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using PoeLogsParser.Enums;
using PoeLogsParser.Models;
using PoeLogsParser.Models.Abstractions;
using PoeLogsParser.Parsers;
using PoeLogsParser.Parsers.Abstractions;
using PoeLogsParser.Services.Abstractions;

namespace PoeLogsParser.Services
{
    public class LogParserService : ILogParserService
    {
        private readonly CurrencyService _currencyService;
        private List<IParser> _parsers;

        private readonly Regex _regLogEntryTag =
            new Regex("\\[(debug|warn|info) Client [0-9]+\\] ", RegexOptions.IgnoreCase);

        public LogParserService()
        {
            _currencyService = new CurrencyService();
            _parsers = new List<IParser>()
            {
                new AreaChangeParser(),
                new PlayerJoinedAreaParser(),
                new TradeChatMessageParser(),
                new GlobalMessageParser(),
                new IncomingTradeParser(),
                new OutgoingTradeParser(),
                new TradeStateParser()
            };
        }

        public ILogEntry Parse(string line)
        {
            var entry = ParseLogEntry(line);

            if (entry == null)
            {
                return default(LogEntry);
            }

            foreach (var parser in _parsers.Where(parser => parser.CanParse(line)))
            {
                try
                {
                    return parser.Execute(entry.Item2, entry.Item1);
                } catch
                {
                    // ignored
                }
            }

            entry.Item1.Types = new[] {LogEntryType.System};

            return entry.Item1;
        }

        private Tuple<LogEntry, string> ParseLogEntry(string line)
        {
            var entry = new LogEntry(line, DateTime.Now, new List<LogEntryType>());

            var dateEndIndex = line.IndexOf(" ", StringComparison.Ordinal);

            if (dateEndIndex == -1)
            {
                return default;
            }

            var timeEndIndex = line.IndexOf(" ", dateEndIndex, StringComparison.Ordinal);

            if (timeEndIndex == -1)
            {
                return default;
            }

            var dateTimeEndIndex = (dateEndIndex + timeEndIndex) - 1;

            if(dateTimeEndIndex == -1) return default;

            var timeStr = line[..dateTimeEndIndex];
            line = line[(dateTimeEndIndex + 1)..];

            var logTagMatch = _regLogEntryTag.Match(line);

            if (!logTagMatch.Success)
            {
                return default;
            }

            var tagStr = line.Substring(logTagMatch.Index + 1,
                line.IndexOf(" ", logTagMatch.Index, StringComparison.Ordinal));
            tagStr = tagStr[..tagStr.IndexOf(" ", StringComparison.Ordinal)];
            line = line[(logTagMatch.Index + logTagMatch.Length)..];

            entry.Time = DateTime.Parse(timeStr);
            entry.Tag = LogEntryTagConveter.Convert(tagStr);

            return new Tuple<LogEntry, string>(entry, line);
        }

        public void AddParser(IParser parser)
        {
            _parsers.Add(parser);
        }

        public void RemoveAllParsers()
        {
            _parsers.Clear();
        }
    }
}