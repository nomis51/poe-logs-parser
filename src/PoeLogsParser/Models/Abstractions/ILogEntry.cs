using System;

namespace PoeLogsParser.Models.Abstractions
{
    public interface ILogEntry
    {
        string Raw { get; set; }
        DateTime Time { get; set; }
    }
}