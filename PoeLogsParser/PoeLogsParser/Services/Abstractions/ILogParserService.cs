using PoeLogsParser.Models.Abstractions;
using PoeLogsParser.Parsers.Abstractions;

namespace PoeLogsParser.Services.Abstractions
{
    public interface ILogParserService : IService
    {
        ILogEntry Parse(string line);
        void AddParser(IParser parser);
        void RemoveAllParsers();
    }
}