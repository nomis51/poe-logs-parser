"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.TradeLogEntry = void 0;
const LogEntry_1 = require("./LogEntry");
class TradeLogEntry extends LogEntry_1.LogEntry {
    constructor(entry) {
        super(entry.raw, entry.time, entry.types, entry.tag);
    }
}
exports.TradeLogEntry = TradeLogEntry;
