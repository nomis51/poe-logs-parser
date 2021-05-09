using System.Collections;
using System.Collections.Generic;

namespace PoeLogsParser.Services.Abstractions
{
    public interface ILogReaderService : IService
    {
        string ReadLastLine();
        string ReadLine(int lineNo);
        IEnumerable<string> ReadLines(int[] linesNo);
    }
}