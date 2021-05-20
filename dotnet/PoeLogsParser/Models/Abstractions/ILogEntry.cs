using System;
using System.Collections.Generic;
using PoeLogsParser.Enums;

namespace PoeLogsParser.Models.Abstractions
{
    public interface ILogEntry
    {
        string Raw { get; set; }
        DateTime Time { get; set; }
        IEnumerable<LogEntryType> Types { get; set; }
        LogEntryTag Tag { get; set; }
    }
}