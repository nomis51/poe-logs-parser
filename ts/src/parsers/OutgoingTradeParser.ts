import { LogEntryType } from "../enums/LogEntryType";
import { TradeParser } from "./TradeParser";

export class OutgoingTradeParser extends TradeParser {
	constructor() {
		super(LogEntryType.Outgoing, /@To .+: Hi, (I would|I'd) like to buy your .+ (listed for|for my) [0-9]+ .+ in .+/gi, [
			/@To /gi,
			/: Hi, (I would|I'd) like to buy your /gi,
			/ (listed for|for my) /gi,
			/ in /gi,
			/ \(stash tab "/gi,
			/"; position: left /gi,
			/, top /gi
		]);
	}
}