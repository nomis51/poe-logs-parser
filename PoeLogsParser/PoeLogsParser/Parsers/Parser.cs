using System;
using System.Runtime.CompilerServices;
using PoeLogsParser.Models;
using PoeLogsParser.Models.Abstractions;
using PoeLogsParser.Parsers.Abstractions;

namespace PoeLogsParser.Parsers
{
    public abstract class Parser : IParser
    {
        public abstract bool CanParse(string line);

        public abstract ILogEntry Parse(string line, LogEntry entry);

        public abstract string Clean(string line);

        public ILogEntry Execute(string line, LogEntry entry)
        {
            line = Clean(line);
            return Parse(line, entry);
        }
    }
}