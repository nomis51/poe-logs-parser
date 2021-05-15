import { LogEntry } from "../../models/entries/LogEntry";

export interface IParser {
	canParse(line: string): boolean;
	parse(line: string, entry: LogEntry): LogEntry;
	clean(line: string): String;
}