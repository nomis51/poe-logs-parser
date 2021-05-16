"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.ChatMessageParser = void 0;
const LogEntryType_1 = require("../enums/LogEntryType");
const ChatMessageLogEntry_1 = require("../models/entries/ChatMessageLogEntry");
class ChatMessageParser {
    constructor(type, regMatch, regsClean) {
        this._type = type;
        this._regMatch = regMatch;
        this._regsClean = regsClean;
    }
    canParse(line) {
        return this._regMatch.test(line);
    }
    parse(line, entry) {
        let chatMessageLogEntry = new ChatMessageLogEntry_1.ChatMessageLogEntry(entry);
        chatMessageLogEntry.types = [LogEntryType_1.LogEntryType.ChatMessage, this._type];
        const parts = line.split(": ");
        chatMessageLogEntry.player = parts[0].replace(/#@%\\$]/gi, "");
        chatMessageLogEntry.message = parts.slice(1).reduce((prev, value) => prev + value, '');
        return chatMessageLogEntry;
    }
    clean(line) {
        return line;
    }
    execute(entry, line) {
        const cleanedline = this.clean(line);
        return this.parse(cleanedline, entry);
    }
}
exports.ChatMessageParser = ChatMessageParser;
