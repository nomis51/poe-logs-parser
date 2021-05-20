using System.Collections.Generic;
using System.Text.RegularExpressions;
using PoeLogsParser.Enums;

namespace PoeLogsParser.Parsers
{
    public class GlobalMessageParser : ChatMessageParser
    {
        public GlobalMessageParser() : base(LogEntryType.Global, new Regex("#.+: .+", RegexOptions.IgnoreCase), new List<Regex>())
        {
        }
    }
}