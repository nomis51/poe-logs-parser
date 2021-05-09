using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using PoeLogsParser.Enums;
using PoeLogsParser.Models;
using PoeLogsParser.Models.Abstractions;
using PoeLogsParser.Services.Abstractions;

namespace PoeLogsParser.Services
{
    public class LogParserService : ILogParserService
    {
        private readonly CurrencyService _currencyService;

        private readonly Regex _regNumber = new Regex("[0-9]+", RegexOptions.IgnoreCase);

        private readonly Regex _regLogEntryTag =
            new Regex("\\[(debug|warn|info) Client [0-9]+\\] ", RegexOptions.IgnoreCase);

        private readonly List<Regex> _regPartsAreaChange = new List<Regex>()
        {
            new Regex(": ", RegexOptions.IgnoreCase),
            new Regex("You have entered ", RegexOptions.IgnoreCase),
            new Regex("\\.", RegexOptions.IgnoreCase)
        };

        private readonly List<Regex> _regPartsPlayerJoinedArea = new List<Regex>()
        {
            new Regex(": ", RegexOptions.IgnoreCase),
            new Regex(" has joined the area.", RegexOptions.IgnoreCase)
        };

        private readonly List<Regex> _regPartsIncomingTrades = new List<Regex>
        {
            new Regex("@From ", RegexOptions.IgnoreCase),
            new Regex(": Hi, (I would|I'd) like to buy your ", RegexOptions.IgnoreCase),
            new Regex(" (listed for|for my) ", RegexOptions.IgnoreCase),
            new Regex(" in ", RegexOptions.IgnoreCase),
            new Regex(" \\(stash tab \"", RegexOptions.IgnoreCase),
            new Regex("\"; position: left ", RegexOptions.IgnoreCase),
            new Regex(", top ", RegexOptions.IgnoreCase)
        };

        private readonly List<Regex> _regPartsOutgoingTrades = new List<Regex>
        {
            new Regex("@To ", RegexOptions.IgnoreCase),
            new Regex(": Hi, (I would|I'd) like to buy your ", RegexOptions.IgnoreCase),
            new Regex(" (listed for|for my) ", RegexOptions.IgnoreCase),
            new Regex(" in ", RegexOptions.IgnoreCase),
            new Regex(" \\(stash tab \"", RegexOptions.IgnoreCase),
            new Regex("\"; position: left ", RegexOptions.IgnoreCase),
            new Regex(", top ", RegexOptions.IgnoreCase)
        };

        private readonly List<Regex> _regsIncomingTrade = new List<Regex>
        {
            new Regex("@From .+: Hi, (I would|I'd) like to buy your .+ (listed for|for my) [0-9]+ .+ in .+",
                RegexOptions.IgnoreCase)
        };

        private readonly List<Regex> _regsOutgoingTrade = new List<Regex>
        {
            new Regex("@To .+: Hi, (I would|I'd) like to buy your .+ (listed for|for my) [0-9]+ .+ in .+",
                RegexOptions.IgnoreCase)
        };

        private readonly Regex _regAreaChange = new Regex("You have entered .+\\.", RegexOptions.IgnoreCase);
        private readonly Regex _regPlayerJoinedArea = new Regex(".+ has joined the area.", RegexOptions.IgnoreCase);
        private readonly Regex _regGlobalMessage = new Regex("#.+: .+", RegexOptions.IgnoreCase);

        public LogParserService()
        {
            _currencyService = new CurrencyService();
        }

        public ILogEntry Parse(string line)
        {
            var entry = ParseLogEntry(line);

            if (entry == null)
            {
                return default(LogEntry);
            }

            if (IsAreaChangeLogEntry(line))
            {
                return ParseAreaChangeLogEntry(entry.Item2, entry.Item1);
            }

            if (IsPlayerJoinedAreaLogEntry(line))
            {
                return ParsePlayerJoinedAreaLogEntry(entry.Item2, entry.Item1);
            }

            if (IsGlobalMessage(line))
            {
                return ParseChatMessage(entry.Item2, entry.Item1, LogEntryType.Global);
            }

            if (IsIncomingTradeLogEntry(line))
            {
                return ParseIncomingTradeLogEntry(entry.Item2, entry.Item1);
            }

            if (IsOutgoingTradeLogEntry(line))
            {
                return ParseOutgoingTradeLogEntry(entry.Item2, entry.Item1);
            }

            entry.Item1.Types = new[] {LogEntryType.System};

            return entry.Item1;
        }

        private bool IsOutgoingTradeLogEntry(string line)
        {
            return _regsOutgoingTrade.Any(reg =>
            {
                var match = reg.Match(line);

                return match.Success;
            });
        }

        private bool IsIncomingTradeLogEntry(string line)
        {
            return _regsIncomingTrade.Any(reg =>
            {
                var match = reg.Match(line);

                return match.Success;
            });
        }

        private bool IsAreaChangeLogEntry(string line)
        {
            return _regAreaChange.IsMatch(line);
        }

        private bool IsPlayerJoinedAreaLogEntry(string line)
        {
            return _regPlayerJoinedArea.IsMatch(line);
        }

        private bool IsGlobalMessage(string line)
        {
            return _regGlobalMessage.IsMatch(line);
        }

        private Tuple<LogEntry, string> ParseLogEntry(string line)
        {
            var entry = new LogEntry(line, DateTime.Now, new List<LogEntryType>());

            var dateEndIndex = line.IndexOf(" ", StringComparison.Ordinal);

            if (dateEndIndex == -1)
            {
                return default(Tuple<LogEntry, string>);
            }

            var timeEndIndex = line.IndexOf(" ", dateEndIndex, StringComparison.Ordinal);

            if (timeEndIndex == -1)
            {
                return default(Tuple<LogEntry, string>);
            }

            var dateTimeEndIndex = (dateEndIndex + timeEndIndex) - 1;

            var timeStr = line[..dateTimeEndIndex];
            line = line[(dateTimeEndIndex + 1)..];

            var logTagMatch = _regLogEntryTag.Match(line);

            if (!logTagMatch.Success)
            {
                return default(Tuple<LogEntry, string>);
            }

            var tagStr = line.Substring(logTagMatch.Index + 1,
                line.IndexOf(" ", logTagMatch.Index, StringComparison.Ordinal));
            tagStr = tagStr[..tagStr.IndexOf(" ", StringComparison.Ordinal)];
            line = line[(logTagMatch.Index + logTagMatch.Length)..];

            entry.Time = DateTime.Parse(timeStr);
            entry.Tag = LogEntryTagConveter.Convert(tagStr);

            return new Tuple<LogEntry, string>(entry, line);
        }

        private ChatMessageLogEntry ParseChatMessage(string line, LogEntry entry, LogEntryType type)
        {
            var chatMessageLogEntry = new ChatMessageLogEntry(entry)
            {
                Types = new[] {LogEntryType.ChatMessage, type}
            };

            var parts = line.Split(": ", StringSplitOptions.RemoveEmptyEntries);
            chatMessageLogEntry.Player = Regex.Replace(parts[0], "#@%$", "");
            chatMessageLogEntry.Message = parts[1..].Aggregate((total, value) => total + value);

            return chatMessageLogEntry;
        }

        private PlayerJoinedAreaLogEntry ParsePlayerJoinedAreaLogEntry(string line, LogEntry entry)
        {
            var playerJoinedAreaLogEntry = new PlayerJoinedAreaLogEntry(entry)
            {
                Types = new[] {LogEntryType.System, LogEntryType.Party}
            };

            line = _regPartsPlayerJoinedArea.Aggregate(line, (current, reg) => reg.Replace(current, ""));

            playerJoinedAreaLogEntry.Player = line;
            return playerJoinedAreaLogEntry;
        }

        private AreaChangeLogEntry ParseAreaChangeLogEntry(string line, LogEntry entry)
        {
            var areaChangeLogEntry = new AreaChangeLogEntry(entry) {Types = new[] {LogEntryType.System}};


            line = _regPartsAreaChange.Aggregate(line, (current, reg) => reg.Replace(current, ""));

            areaChangeLogEntry.Area = line;
            return areaChangeLogEntry;
        }

        private TradeLogEntry ParseIncomingTradeLogEntry(string line, LogEntry entry)
        {
            return ParseTradeLogEntry(line, entry, true);
        }

        private TradeLogEntry ParseOutgoingTradeLogEntry(string line, LogEntry entry)
        {
            return ParseTradeLogEntry(line, entry, false);
        }

        private TradeLogEntry ParseTradeLogEntry(string line, LogEntry entry, bool isIncoming = true)
        {
            var tradeLogEntry = new TradeLogEntry(entry)
            {
                Types = new List<LogEntryType>
                {
                    isIncoming ? LogEntryType.Incoming : LogEntryType.Outgoing,
                    LogEntryType.Trade,
                    LogEntryType.Whisper
                }
            };

            line = line[(line.IndexOf("@", StringComparison.Ordinal))..];

            line = (isIncoming ? _regPartsIncomingTrades : _regPartsOutgoingTrades).Aggregate(line,
                (current, reg) => reg.Replace(current, "#"));

            line = line[..^1];

            var parts = line.Split("#", StringSplitOptions.RemoveEmptyEntries);

            tradeLogEntry.Player = parts[0];

            if (_regNumber.IsMatch(parts[1]))
            {
                var itemParts = parts[1].Split(" ");

                tradeLogEntry.Item = new Item()
                {
                    Quantity = Convert.ToInt32(itemParts[0]),
                    Name = itemParts[1..].Aggregate((total, value) => $"{total} {value}")
                };
            } else
            {
                tradeLogEntry.Item = new Item()
                {
                    Name = parts[1]
                };
            }

            var priceParts = parts[2].Split(" ");
            var realCurrencyName = _currencyService.GetRealName(priceParts[1]);
            var loweredCurrencyName = priceParts[1].ToLower();

            tradeLogEntry.Price = new Price()
            {
                Value = Convert.ToDouble(priceParts[0]),
                Currency = realCurrencyName,
                NormalizedCurrency = loweredCurrencyName,
                ImageLink = _currencyService.GetCurrencyImageLink(loweredCurrencyName)
            };

            tradeLogEntry.League = parts[3];

            if (parts.Length > 4)
            {
                tradeLogEntry.Location = new StashLocation()
                {
                    StashTab = parts[4],
                    Left = Convert.ToInt32(parts[5]),
                    Top = Convert.ToInt32(parts[6])
                };
            }

            return tradeLogEntry;
        }
    }
}