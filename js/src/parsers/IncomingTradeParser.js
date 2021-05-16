"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.IncomingTradeParser = void 0;
const LogEntryType_1 = require("../enums/LogEntryType");
const TradeParser_1 = require("./TradeParser");
class IncomingTradeParser extends TradeParser_1.TradeParser {
    constructor() {
        super(LogEntryType_1.LogEntryType.Incoming, /@From .+: Hi, (I would|I'd) like to buy your .+ (listed for|for my) [0-9]+ .+ in .+/gi, [
            /@From /gi,
            /: Hi, (I would|I'd) like to buy your /gi,
            / (listed for|for my) /gi,
            / in /gi,
            / \(stash tab "/gi,
            /"; position: left /gi,
            /, top /gi
        ]);
    }
}
exports.IncomingTradeParser = IncomingTradeParser;
