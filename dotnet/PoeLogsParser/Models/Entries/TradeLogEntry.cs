using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using PoeLogsParser.Enums;

namespace PoeLogsParser.Models
{
    public class TradeLogEntry : LogEntry
    {
        public string Player { get; set; }
        public Item Item { get; set; }
        public string League { get; set; }
        public Price Price { get; set; }
        public StashLocation Location { get; set; }
        public string Notes { get; set; }

        public TradeLogEntry(LogEntry entry) : base(entry.Raw, entry.Time, entry.Types, entry.Tag)
        {
        }
        
       
    }
}