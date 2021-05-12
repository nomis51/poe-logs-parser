using System;
using System.Collections;
using System.Collections.Generic;
using PoeLogsParser.Enums;
using PoeLogsParser.Models.Abstractions;

namespace PoeLogsParser.Models
{
    public class LogEntry : ILogEntry
    {
        public string Raw { get; set; }
        public DateTime Time { get; set; }
        public IEnumerable<LogEntryType> Types { get; set; }
        public LogEntryTag Tag { get; set; }

        public LogEntry(string raw, DateTime time, IEnumerable<LogEntryType> types,
            LogEntryTag tag = LogEntryTag.Info)
        {
            Raw = raw;
            Time = time;
            Types = types;
            Tag = tag;
        }
    }
}