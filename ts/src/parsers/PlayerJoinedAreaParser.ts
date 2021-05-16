import { LogEntryType } from "../enums/LogEntryType";
import { LogEntry } from "../models/entries/LogEntry";
import { PlayerJoinedAreaLogEntry } from "../models/entries/PlayerJoinedAreaLogEntry";
import { IParser } from "./abstractions/IParser";

export class PlayerJoinedAreaParser implements IParser {
	private _regMatch: RegExp = /.+ has joined the area./gi
	private _regsClean: RegExp[] = [
		/: /gi,
		/ has joined the area./gi
	];

	constructor() {
	}

	canParse(line: string): boolean {
		return this._regMatch.test(line);
	}

	parse(line: string, entry: LogEntry): PlayerJoinedAreaLogEntry {
		let playerJoinedAreaLogEntry = new PlayerJoinedAreaLogEntry(entry);

		playerJoinedAreaLogEntry.types = [LogEntryType.System, LogEntryType.Party, LogEntryType.JoinArea];

		playerJoinedAreaLogEntry.player = line;
		return playerJoinedAreaLogEntry;
	}

	clean(line: string): String {
		this._regsClean.forEach((reg) => line = line.replace(reg, ""));
		return line;
	}

	execute(entry: LogEntry, line: string): LogEntry {
		const cleanedline = this.clean(line);
		return this.parse(cleanedline as string, entry);
	}
}