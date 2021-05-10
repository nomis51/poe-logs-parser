using System.Collections;
using System.Collections.Generic;

namespace PoeLogsParser.Services.Abstractions
{
    public interface ILogReaderService : IService
    {
        delegate void NewLogEntryEvent(string line);
        event NewLogEntryEvent NewLogEntry;
        
        
        string ReadLastLine();
        string ReadLine(int lineNo);
        IEnumerable<string> ReadLines(int[] linesNo);
    }
}