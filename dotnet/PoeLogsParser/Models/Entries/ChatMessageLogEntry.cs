using System;
using System.Collections.Generic;
using PoeLogsParser.Enums;

namespace PoeLogsParser.Models
{
    public class ChatMessageLogEntry : LogEntry
    {
        public string Player { get; set; }
        public string Message { get; set; }
        
        public ChatMessageLogEntry(LogEntry entry) : base(entry.Raw, entry.Time, entry.Types, entry.Tag)
        {
        }
    }
}