import { LogEntryTag } from "../../enums/LogEntryTag";
import { LogEntryType } from "../../enums/LogEntryType";

export class LogEntry {
	public raw!: String;
	public time!: Date;
	public types!: LogEntryType[];
	public tag!: LogEntryTag;

	constructor(raw: string, time: Date, types: LogEntryType[], tag: LogEntryTag) {
		this.raw = raw;
		this.time = time;
		this.types = types;
		this.tag = tag;
	}
};