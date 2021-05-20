import { LogEntryType } from "../enums/LogEntryType";
import { ChatMessageLogEntry } from "../models/entries/ChatMessageLogEntry";
import { LogEntry } from "../models/entries/LogEntry";
import { IParser } from "./abstractions/IParser";

export class ChatMessageParser implements IParser {
	private _regMatch: RegExp;
	private _regsClean: RegExp[];
	private _type: LogEntryType;

	constructor(type: LogEntryType, regMatch: RegExp, regsClean: RegExp[]) {
		this._type = type;
		this._regMatch = regMatch;
		this._regsClean = regsClean;
	}

	canParse(line: string): boolean {
		return this._regMatch.test(line);
	}

	parse(line: string, entry: LogEntry): ChatMessageLogEntry {
		let chatMessageLogEntry = new ChatMessageLogEntry(entry);

		chatMessageLogEntry.types = [LogEntryType.ChatMessage, this._type];

		const parts = line.split(": ");
		chatMessageLogEntry.player = parts[0].replace(/#@%\\$]/gi, "");
		chatMessageLogEntry.message = parts.slice(1).reduce((prev, value) => prev + value, '');

		return chatMessageLogEntry;
	}

	clean(line: string): String {
		return line;
	}

	execute(entry: LogEntry, line: string): LogEntry {
		const cleanedline = this.clean(line);
		return this.parse(cleanedline as string, entry);
	}

}