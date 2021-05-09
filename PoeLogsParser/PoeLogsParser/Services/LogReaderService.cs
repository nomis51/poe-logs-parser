using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PoeLogsParser.Exceptions;
using PoeLogsParser.Models;
using PoeLogsParser.Services.Abstractions;

namespace PoeLogsParser.Services
{
    public class LogReaderService : ILogReaderService
    {
        #region Events

        public delegate void NewLogEntryEvent(string line);

        public event NewLogEntryEvent OnNewLogEntry;

        #endregion

        #region Constants

        private readonly string[] _poeProcesses = new[]
        {
            "PathOfExile_x64",
            "PathOfExile_x64Steam"
        };

        #endregion

        #region Members

        private string _logFilePath;
        private bool _fileOpened = false;
        private FileStream _file;
        private long _endOfDile = 0;

        #endregion

        #region Constructors

        public LogReaderService(string logFilePath)
        {
            _logFilePath = logFilePath;
            Watch();
        }

        public LogReaderService()
        {
            FindLogFilePath();
            Watch();
        }

        #endregion

        private void FindLogFilePath()
        {
            var processes = Process.GetProcesses();

            foreach (var process in processes)
            {
                if (!_poeProcesses.Contains(process.ProcessName) || process.HasExited) continue;
                try
                {
                    if (process.MainModule?.FileName != null)
                        _logFilePath =
                            $"{(process.MainModule?.FileName)[..process.MainModule.FileName.LastIndexOf("\\", StringComparison.Ordinal)]}\\logs\\Client.txt";
                } catch (Exception e)
                {
                    throw new CannotFindLogFileException(process.ProcessName);
                }
            }
        }

        private async void Watch()
        {
            while (true)
            {
                var newLines = new List<string>();

                do
                {
                    try
                    {
                        await Task.Delay(500);

                        newLines = this.ReadNewLines();
                    } catch
                    {
                        // ignored
                    }
                } while (!newLines.Any());

                foreach (var line in newLines)
                {
                    OnOnNewLogEntry(line);
                }
            }
            // ReSharper disable once FunctionNeverReturns
        }

        private void SetEndOfFile()
        {
            var file = File.Open(_logFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            _endOfDile = file.Length - 1;
            file.Close();
        }

        private List<string> ReadNewLines()
        {
            var lines = new List<string>();

            var currentPosition = _endOfDile;

            SetEndOfFile();

            if (currentPosition >= _endOfDile)
            {
                return lines;
            }

            var file = File.Open(_logFilePath, FileMode.Open, FileAccess.Read,
                FileShare.ReadWrite);
            file.Position = currentPosition;
            var reader = new StreamReader(file);

            var line = reader.ReadLine();
            while (!reader.EndOfStream)
            {
                if (!string.IsNullOrEmpty(line))
                {
                    lines.Add(line);
                }
            }

            reader.Close();
            file.Close();

            return lines;
        }

        private bool OpenLogFile()
        {
            if (string.IsNullOrEmpty(_logFilePath))
            {
                return false;
            }

            FindLogFilePath();

           // new FileStream(@"c:\file.txt", FileMode.Open, FileAccess.Read)

            return true;
        }

        public string ReadLastLine()
        {
            throw new System.NotImplementedException();
        }

        public string ReadLine(int lineNo)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<string> ReadLines(int[] linesNo)
        {
            throw new System.NotImplementedException();
        }


        protected virtual void OnOnNewLogEntry(string line)
        {
            OnNewLogEntry?.Invoke(line);
        }
    }
}