import { LogEntryType } from "../enums/LogEntryType";
import { ChatMessageParser } from "./ChatMessageParser";

export class GlobalMessageParser extends ChatMessageParser {
	constructor() {
		super(LogEntryType.Global, /#.+: .+/gi, [])
	}
}