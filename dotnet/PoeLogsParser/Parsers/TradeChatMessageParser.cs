using System.Collections.Generic;
using System.Text.RegularExpressions;
using PoeLogsParser.Enums;

namespace PoeLogsParser.Parsers
{
    public class TradeChatMessageParser : ChatMessageParser
    {
        public TradeChatMessageParser() : base(LogEntryType.Trade, new Regex("\\$.+: .+", RegexOptions.IgnoreCase), new List<Regex>())
        {
        }
    }
}