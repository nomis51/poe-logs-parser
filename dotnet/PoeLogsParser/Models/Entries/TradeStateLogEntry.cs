using System;
using System.Collections.Generic;
using PoeLogsParser.Enums;
using PoeLogsParser.Models.Abstractions;

namespace PoeLogsParser.Models
{
    public class TradeStateLogEntry : LogEntry
    {
        public bool IsAccepted { get; set; }
        public bool IsCancelled { get; set; }

        public TradeStateLogEntry(ILogEntry entry) : base(entry.Raw, entry.Time, entry.Types, entry.Tag)
        {
        }
    }
}