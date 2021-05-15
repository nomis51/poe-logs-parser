import { LogEntry } from './LogEntry'

export class AreaChangeLogEntry extends LogEntry {
	public area!: String;

	constructor(entry: LogEntry) {
		super();
		this.raw = entry.raw;
		this.tag = entry.tag;
		this.time = entry.time;
	}
}