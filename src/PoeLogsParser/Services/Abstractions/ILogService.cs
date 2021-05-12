using System.Collections;
using System.Collections.Generic;
using PoeLogsParser.Models.Abstractions;
using PoeLogsParser.Parsers.Abstractions;

namespace PoeLogsParser.Services.Abstractions
{
    public interface ILogService : IService
    {
         delegate void NewLogEntryEvent(ILogEntry logEntry);
         event NewLogEntryEvent NewLogEntry;
        
        void AddParser(IParser parser);
        void AddParsers(IEnumerable<IParser> parsers);
        void RemoveAllParsers();
        ILogEntry ReadLastLine();
        ILogEntry ReadLine(int lineNo);
        Dictionary<int, ILogEntry> ReadLines(int[] linesNo);
    }
}