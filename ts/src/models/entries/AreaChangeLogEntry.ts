import { LogEntry } from './LogEntry'

export class AreaChangeLogEntry extends LogEntry {
	public area!: String;

	constructor(entry: LogEntry) {
		super(entry.raw as string, entry.time, entry.types, entry.tag);
	}
}