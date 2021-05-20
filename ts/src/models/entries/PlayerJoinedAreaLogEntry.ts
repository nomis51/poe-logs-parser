import { LogEntry } from "./LogEntry";

export class PlayerJoinedAreaLogEntry extends LogEntry {
	public player!: String;

	constructor(entry: LogEntry) {
		super(entry.raw as string, entry.time, entry.types, entry.tag);
	}
}