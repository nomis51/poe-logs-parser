using System.Collections;
using System.Collections.Generic;
using PoeLogsParser.Models;
using PoeLogsParser.Models.Abstractions;
using PoeLogsParser.Parsers.Abstractions;

namespace PoeLogsParser.Services.Abstractions
{
    public interface ILogService : IService
    {
        delegate void NewLogEntryEvent(ILogEntry logEntry);

        event NewLogEntryEvent NewLogEntry;

        delegate void NewTradeLogEntryEvent(TradeLogEntry logEntry);

        event NewTradeLogEntryEvent NewTradeLogEntry;

        delegate void NewPlayerJoinedAreaLogEntryEvent(PlayerJoinedAreaLogEntry logEntry);

        event NewPlayerJoinedAreaLogEntryEvent NewPlayerJoinedAreaLogEntry;

        delegate void NewAreaChangeLogEntryEvent(AreaChangeLogEntry logEntry);

        event NewAreaChangeLogEntryEvent NewAreaChangeLogEntry;

        delegate void NewChatMessageLogEntryEvent(ChatMessageLogEntry logEntry);

        event NewChatMessageLogEntryEvent NewChatMessageLogEntry;
        
        delegate void NewTradeStateLogEntryEvent(TradeStateLogEntry logEntry);

        event NewTradeStateLogEntryEvent NewTradeStateLogEntry;

        void AddParser(IParser parser);
        void AddParsers(IEnumerable<IParser> parsers);
        void RemoveAllParsers();
        ILogEntry ReadLastLine();
        ILogEntry ReadLine(int lineNo);
        Dictionary<int, ILogEntry> ReadLines(int[] linesNo);
        ILogEntry Parse(string line);
    }
}