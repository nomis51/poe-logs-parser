import { LogEntryType } from "../enums/LogEntryType";
import { AreaChangeLogEntry } from "../models/entries/AreaChangeLogEntry";
import { LogEntry } from "../models/entries/LogEntry";
import { IParser } from "./abstractions/IParser";

export class AreaChangeParser implements IParser {

	private _regMatch: RegExp = /You have entered .+\\./gi
	private _regsClean: RegExp[] = [
		/: /gi,
		/You have entered /gi,
		/\\./gi
	]

	execute(entry: LogEntry, line: string): LogEntry {
		const cleanedline = this.clean(line);
		return this.parse(cleanedline as string, entry);
	}

	canParse(line: string): boolean {
		return this._regMatch.test(line);
	}

	parse(line: string, entry: LogEntry): AreaChangeLogEntry {
		let areaChangeEntry = new AreaChangeLogEntry(entry);

		areaChangeEntry.types = [LogEntryType.System, LogEntryType.ChangeArea];
		areaChangeEntry.area = line;

		return areaChangeEntry;
	}

	clean(line: string): String {
		this._regsClean.forEach((reg) => line = line.replace(reg, ""));
		return line;
	}

}