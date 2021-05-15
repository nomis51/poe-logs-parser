import { LogEntry } from "./LogEntry";

export class ChatMessageLogEntry extends LogEntry {
	public player!: String;
	public message!: String;
}