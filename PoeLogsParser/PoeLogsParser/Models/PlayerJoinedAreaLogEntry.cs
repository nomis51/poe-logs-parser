namespace PoeLogsParser.Models
{
    public class PlayerJoinedAreaLogEntry : LogEntry
    {
        public string Player { get; set; }

        public PlayerJoinedAreaLogEntry(LogEntry entry) : base(entry.Raw, entry.Time, entry.Types, entry.Tag)
        {
        }
    }
}