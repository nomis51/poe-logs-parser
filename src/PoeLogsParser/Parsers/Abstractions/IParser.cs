using System.Collections.Generic;
using System.Text.RegularExpressions;
using PoeLogsParser.Models;
using PoeLogsParser.Models.Abstractions;

namespace PoeLogsParser.Parsers.Abstractions
{
    public interface IParser
    {
        bool CanParse(string line);
        ILogEntry Parse(string line, LogEntry entry);
        string Clean(string line);
        ILogEntry Execute(string line, LogEntry entry);
    }
}