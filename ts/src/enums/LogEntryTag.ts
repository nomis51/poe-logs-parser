export enum LogEntryTag {
	Warn,
	Info,
	Debug
}

export function ConvertLogEntryTag(str: string): LogEntryTag {
	switch (str.toLowerCase()) {
		case 'warn':
			return LogEntryTag.Warn;

		case 'debug':
			return LogEntryTag.Debug;

		default:
			return LogEntryTag.Info;
	}
}