import { LogEntry } from "./LogEntry";

export class ChatMessageLogEntry extends LogEntry {
	public player!: String;
	public message!: String;

	constructor(entry: LogEntry) {
		super(entry.raw as string, entry.time, entry.types, entry.tag);
	}
}