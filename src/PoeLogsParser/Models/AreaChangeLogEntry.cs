using System;
using System.Collections.Generic;
using PoeLogsParser.Enums;

namespace PoeLogsParser.Models
{
    public class AreaChangeLogEntry : LogEntry
    {
        public string Area { get; set; }
        
        public AreaChangeLogEntry(LogEntry entry) : base(entry.Raw, entry.Time, entry.Types, entry.Tag)
        {
        }
    }
}