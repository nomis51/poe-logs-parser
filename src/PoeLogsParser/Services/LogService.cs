using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PoeLogsParser.Models;
using PoeLogsParser.Models.Abstractions;
using PoeLogsParser.Parsers.Abstractions;
using PoeLogsParser.Services.Abstractions;

namespace PoeLogsParser.Services
{
    public class LogService : ILogService
    {
        public event ILogService.NewLogEntryEvent NewLogEntry;
        
        private readonly ILogReaderService _logReaderService;
        private readonly ILogParserService _logParserService;

        public LogService(string logFilePath = "")
        {
            _logReaderService = string.IsNullOrEmpty(logFilePath)
                ? new LogReaderService()
                : new LogReaderService(logFilePath);
            _logParserService = new LogParserService();
            
            _logReaderService.NewLogEntry += LogReaderService_OnNewLogEntry;
        }

        private void LogReaderService_OnNewLogEntry(string line)
        {
            if (string.IsNullOrEmpty(line)) return;
            
            var entry = _logParserService.Parse(line);

            if (entry == null) return;
            
            OnNewLogEntry(entry);
        }

        public void AddParser(IParser parser)
        {
            _logParserService.AddParser(parser);
        }

        public void AddParsers(IEnumerable<IParser> parsers)
        {
            var enumerable = parsers as IParser[] ?? parsers.ToArray();

            if (!enumerable.Any()) return;

            foreach (var parser in enumerable)
            {
                _logParserService.AddParser(parser);
            }
        }

        public void RemoveAllParsers()
        {
            _logParserService.RemoveAllParsers();
        }

        public ILogEntry ReadLastLine()
        {
            var line = _logReaderService.ReadLastLine();

            return string.IsNullOrEmpty(line) ? default(ILogEntry) : _logParserService.Parse(line);
        }

        public ILogEntry ReadLine(int lineNo)
        {
            if (lineNo < 0) return default(ILogEntry);

            var line = _logReaderService.ReadLine(lineNo);

            return string.IsNullOrEmpty(line) ? default(ILogEntry) : _logParserService.Parse(line);
        }

        public Dictionary<int, ILogEntry> ReadLines(int[] linesNo)
        {
            var entries = new Dictionary<int, ILogEntry>();
            var lines = _logReaderService.ReadLines(linesNo).ToList();

            for (var i = 0; i < linesNo.Length; ++i)
            {
                entries.Add(linesNo[i], _logParserService.Parse(lines[i]));
            }

            return entries;
        }

        public ILogEntry Parse(string line)
        {
            return _logParserService.Parse(line);
        }

        protected virtual void OnNewLogEntry(ILogEntry logEntry)
        {
            NewLogEntry?.Invoke(logEntry);
        }
    }
}