import { Subject } from "rxjs";
import { LogEntry } from "../models/entries/LogEntry";
import { IParser } from "../parsers/abstractions/IParser";
import { LogParserService } from "./LogParserService";
import { LogReaderService } from "./LogReaderService";

export class LogService {
	public newLogEntry: Subject<LogEntry>;
	public newTradeLogEntry: Subject<LogEntry>;
	public newPlayerJoinedAreaLogEntry: Subject<LogEntry>;
	public newAreaChangeLogEntry: Subject<LogEntry>;
	public newChatMessageLogEntry: Subject<LogEntry>;

	private _logParserService: LogParserService;
	private _logReaderService: LogReaderService;

	constructor(logFilePath: string | undefined = undefined) {
		this.newLogEntry = new Subject();
		this.newTradeLogEntry = new Subject();
		this.newPlayerJoinedAreaLogEntry = new Subject();
		this.newAreaChangeLogEntry = new Subject();
		this.newChatMessageLogEntry = new Subject();

		this._logParserService = new LogParserService();
		this._logReaderService = new LogReaderService(logFilePath);

		this._logReaderService.newLogEntry.subscribe(this.onNewLogEntry);
	}

	private onNewLogEntry(line: string): void {
		if (!line) return;

		const entry = this._logParserService.parse(line);

		if (!entry) return;

		this.newLogEntry.next(entry);
	}

	public addParser(parser: IParser): void {
		this._logParserService.addParser(parser);
	}

	public addParsers(parsers: IParser[] = []): void {
		if (parsers.length === 0) return;

		for (const parser of parsers) {
			this._logParserService.addParser(parser);
		}
	}

	public removeAllParsers(): void {
		this._logParserService.removeAllParsers();
	}

	public parse(line: string): LogEntry | undefined {
		return this._logParserService.parse(line);
	}
}