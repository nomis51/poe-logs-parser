# Path of Exile Logs Parser
PoeLogsParser is a simple, but powerful .NET library that let you easily parse Path of Exile Client.txt log file.

## Requirements
- .NET 5.0

## Features
- Automatically parse new log entries that gets added to the Client.txt file when the game runs and notify with an event-communication system
- Parse log entries according to their type (e.g. Global chat message, whisper, trade whisper message, you know the classis "Hi, I would like to buy your ...", etc.)
- You can add your own custom parsers (so the library can parse some log entries according to your needs)
- You can ask for a manual log entry parsing yourself at any time
- You can read specific log entries line (e.g. line #3467, lines #23, #7854 and #4, the last line, etc)

## API


## Examples
### Use the library
```csharp
// Initialize the main log service
// You can specify or not, a Client.txt path
ILogService ls = new LogService("./Client.txt");
// or
ILogService ls = new LogService();
// If no path is provided, the LogService is going to try to find a Path of Exile
// process on the system to find the Client.txt file associated with it
```

### Simple line reads
```csharp
ILogService ls = new LogService("./Client.txt");
// Read a specific line
ILogEntry entry = ls.ReadLine(14);

// Read specific lines (this the raw line, not parsed)
// <line number, the entry>
Dictionary<int, ILogEntry> lines = ls.ReadLines(new []{ 2, 4563, 74});
// [ { 2, <ILogEntry> }, { 4563, <ILogEntry> }, ... ]

// Read the last line
ILogEntry entry = ls.ReadLastLine();
```
### Manual parsing
```csharp
ILogService ls = new LogService();
string line = "2020/07/25 16:56:25 237826523 b46 [INFO Client 67234] @From wannaBeFamous: Hi, I'd like to buy your 1 Mirror of Kalandra for my 2 Chaos Orb in Harvest";
ILogEntry entry = ls.Parse(line);
```

### Listening to new log entry
When new log entries are added to the Client.txt file by the game, you gonna be notified.

```csharp
ILogService ls = new LogService("./Client.txt");
ls.NewLogEntry += delegate(ILogEntry entry)  
{  
    // use the entry...
};
```

### Using custom parsers
You can remove all the default parsers if you want. You can add new ones and mix them with default ones.
Keep in mind, that the order the parser got added into the `LogService`,  is the order in which the parsers are going to run, so if parser #1 consumme the log entry cause it was able to parse it (Let's it was a global message log entry and the parse #1 is the one that parse Global messages), the other parsers are going to be ignored.

```csharp
public class TradeChatMinionStuffLogEntry : LogEntry {
	public string TypeOfMinion { get; set; }
    
    public TradeChatMinionStuffLogEntry(LogEntry entry) : base(entry.Raw, entry.Time, entry.Types, entry.Tag) { }
}

public class LookForTradeChatMessageAboutMinionsParser : Parser  
{  
    // Regex to verify that the log entry contains the informations we are looking for
    private readonly Regex _regMatch = new Regex("$.+: .*minion.*", RegexOptions.IgnoreCase);  
  
    // Used to "clean up" the string before parsing
    // Here, we don't want the usual "WTB", "WTS" that player use
    // at the beginning of their message
    private readonly List<Regex> _regsClean = new List<Regex>()  
	{  
        new Regex("WTB", RegexOptions.IgnoreCase),  
        new Regex("WTS", RegexOptions.IgnoreCase) 
    };  
  
    // Define is the "line" can be parsed (Is it the type of line we are looking for,
    // for this parser ?)
    public override bool CanParse(string line)  
    {  
        return _regMatch.IsMatch(line);  
    }  
    
    // Clean up the line before parsing (Remove the infos we don't need) 
    public override string Clean(string line)  
    {  
        return _regsClean.Aggregate(line, (current, reg) => reg.Replace(current, ""));  
    } 
  
    // Parse the actual line into a custom ILogEntry
    public override ILogEntry Parse(string line, LogEntry entry)  
    {  
        var minionEntry = new TradeChatMinionStuffLogEntry(entry) { 
	        Types = new[] {LogEntryType.ChatMessage, LogEntryType.Trade}
        };  

		if(Regex.IsMatch(line, "golem", RegexOptions.IgnoreCase))
		{
			minionEntry.TypeOfMinion = "It's about golems!!!";
		}

		return minionEntry;
    }  
} 


// In you app somewhere...
public class YouApp {
	public YouApp() {
		ILogService ls = new LogService("./Client.txt");
		ls.AddParser(new LookForTradeChatMessageAboutMinionsParser());
		// Now, your parse is going to be used the future log entries
	}
}

```

# TODO
- Implement a TypeScript / JavaScript version
- Add more Parsers

Tested on Linux and Windows.
