"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.GlobalMessageParser = void 0;
const LogEntryType_1 = require("../enums/LogEntryType");
const ChatMessageParser_1 = require("./ChatMessageParser");
class GlobalMessageParser extends ChatMessageParser_1.ChatMessageParser {
    constructor() {
        super(LogEntryType_1.LogEntryType.Global, /#.+: .+/gi, []);
    }
}
exports.GlobalMessageParser = GlobalMessageParser;
