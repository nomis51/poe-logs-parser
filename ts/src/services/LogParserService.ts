import { IParser } from '../parsers/abstractions/IParser';
import { AreaChangeParser } from '../parsers/AreaChangeParser'
import { PlayerJoinedAreaParser } from '../parsers/PlayerJoinedAreaParser'
import { GlobalMessageParser } from '../parsers/GlobalMessageParser'
import { IncomingTradeParser } from '../parsers/IncomingTradeParser'
import { OutgoingTradeParser } from '../parsers/OutgoingTradeParser'
import { LogEntry } from '../models/entries/LogEntry';
import { LogEntryTag, ConvertLogEntryTag } from '../enums/LogEntryTag';
import { LogEntryType } from '../enums/LogEntryType';

export class LogParserService {
	private _regLogEntryTag: RegExp = /\[(debug|warn|info) Client [0-9]+\] /gi;
	private _parsers: IParser[];

	constructor() {
		this._parsers = [
			new AreaChangeParser(),
			new PlayerJoinedAreaParser(),
			new GlobalMessageParser(),
			new IncomingTradeParser(),
			new OutgoingTradeParser()
		];
	}

	public parse(line: string): LogEntry | undefined {
		let entry = this.parseLogEntry(line);

		if (!entry) return;

		for (let parser of this._parsers.filter(p => p.canParse(line))) {
			try {
				return parser.execute(entry.entry, entry.line as string);
			} catch { }
		}

		entry.entry.types = [LogEntryType.System];
		return entry.entry;
	}

	public addParser(parser: IParser): void {
		this._parsers.push(parser);
	}

	public removeAllParsers(): void {
		this._parsers = [];
	}

	private parseLogEntry(line: string): { entry: LogEntry, line: String } | undefined {
		var entry = new LogEntry(line, new Date(), [], LogEntryTag.Info);

		var dateEndIndex = line.indexOf(" ");

		if (dateEndIndex == -1) {
			return;
		}

		var timeEndIndex = line.indexOf(" ", dateEndIndex);

		if (timeEndIndex == -1) {
			return;
		}

		var dateTimeEndIndex = (dateEndIndex + timeEndIndex) - 1;

		var timeStr = line.substring(0, dateTimeEndIndex);
		line = line.substring((dateTimeEndIndex + 1));

		var logTagMatch = this._regLogEntryTag.exec(line);

		if (!logTagMatch) {
			return;
		}

		var tagStr = line.substring(logTagMatch.index + 1,
			line.indexOf(" ", logTagMatch.index));
		tagStr = tagStr.substring(0, tagStr.indexOf(" "));
		line = line.substring(logTagMatch.index + logTagMatch.length);

		entry.time = new Date(timeStr);
		entry.tag = ConvertLogEntryTag(tagStr);

		return { entry, line };
	}
}