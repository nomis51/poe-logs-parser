"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.PlayerJoinedAreaLogEntry = void 0;
const LogEntry_1 = require("./LogEntry");
class PlayerJoinedAreaLogEntry extends LogEntry_1.LogEntry {
    constructor(entry) {
        super(entry.raw, entry.time, entry.types, entry.tag);
    }
}
exports.PlayerJoinedAreaLogEntry = PlayerJoinedAreaLogEntry;
