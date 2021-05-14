using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PoeLogsParser.Models;
using PoeLogsParser.Models.Abstractions;
using PoeLogsParser.Parsers.Abstractions;
using PoeLogsParser.Services.Abstractions;

namespace PoeLogsParser.Services
{
	/// <summary>
	/// Represent the service that read and parse Path of Exile Client.txt log file
	/// </summary>
	public class LogService : ILogService
	{
		#region Events
		/// <summary>
		/// Generic event for new log entry
		/// </summary>
		public event ILogService.NewLogEntryEvent NewLogEntry;
		#endregion

		#region Members
		/// <summary>
		/// Service to read Client.txt file
		/// </summary>
		private readonly ILogReaderService _logReaderService;
		/// <summary>
		/// Service to parse Client.txt log entries
		/// </summary>
		private readonly ILogParserService _logParserService;
		#endregion

		#region Constructors
		/// <summary>
		/// 
		/// </summary>
		/// <param name="logFilePath">Client.txt absolute path. If empty, LogService is going to try to find it itself.</param>
		public LogService(string logFilePath = "")
		{
			_logReaderService = string.IsNullOrEmpty(logFilePath)
				? new LogReaderService()
				: new LogReaderService(logFilePath);
			_logParserService = new LogParserService();

			_logReaderService.NewLogEntry += LogReaderService_OnNewLogEntry;
		}
		#endregion

		/// <summary>
		/// When a new log entry has been red by the LogReaderService
		/// </summary>
		/// <param name="line">Log entry</param>
		private void LogReaderService_OnNewLogEntry(string line)
		{
			if (string.IsNullOrEmpty(line)) return;

			var entry = _logParserService.Parse(line);

			if (entry == null) return;

			OnNewLogEntry(entry);
		}

		/// <summary>
		/// Add a new parser to the list of parsers used to parse log entries
		/// </summary>
		/// <param name="parser">A IParser used to parsed log entries</param>
		public void AddParser(IParser parser)
		{
			_logParserService.AddParser(parser);
		}

		/// <summary>
		/// Add new parsers to the list of parsers used to parse log entries
		/// </summary>
		/// <param name="parsers">A list of IParsers used to parse log entries</param>
		public void AddParsers(IEnumerable<IParser> parsers)
		{
			var enumerable = parsers as IParser[] ?? parsers.ToArray();

			if (!enumerable.Any()) return;

			foreach (var parser in enumerable)
			{
				_logParserService.AddParser(parser);
			}
		}

		/// <summary>
		/// Remove all parsers used by LogParserService
		/// </summary>
		public void RemoveAllParsers()
		{
			_logParserService.RemoveAllParsers();
		}

		/// <summary>
		/// Reads and parse the last line from the Client.txt file
		/// </summary>
		/// <returns>A log entry</returns>
		public ILogEntry ReadLastLine()
		{
			var line = _logReaderService.ReadLastLine();

			return string.IsNullOrEmpty(line) ? default(ILogEntry) : _logParserService.Parse(line);
		}

		/// <summary>
		/// Reads and parse the line at lineNo number from the Client.txt file
		/// </summary>
		/// <param name="lineNo">Line number</param>
		/// <returns>A log entry</returns>
		public ILogEntry ReadLine(int lineNo)
		{
			if (lineNo < 0) return default(ILogEntry);

			var line = _logReaderService.ReadLine(lineNo);

			return string.IsNullOrEmpty(line) ? default(ILogEntry) : _logParserService.Parse(line);
		}

		/// <summary>
		/// Reads and parse the lines from the Client.txt file
		/// </summary>
		/// <param name="linesNo">List of line numbers</param>
		/// <returns>Key pairs values of line numbers and log entries</returns>
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

		/// <summary>
		/// Parse a string log entry
		/// </summary>
		/// <param name="line">A string log entry</param>
		/// <returns>A parsed log entry</returns>
		public ILogEntry Parse(string line)
		{
			return _logParserService.Parse(line);
		}

		/// <summary>
		/// When a new log entry has been parsed
		/// </summary>
		/// <param name="logEntry">A log entry</param>
		protected virtual void OnNewLogEntry(ILogEntry logEntry)
		{
			NewLogEntry?.Invoke(logEntry);
		}
	}
}