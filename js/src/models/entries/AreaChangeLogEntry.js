"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.AreaChangeLogEntry = void 0;
const LogEntry_1 = require("./LogEntry");
class AreaChangeLogEntry extends LogEntry_1.LogEntry {
    constructor(entry) {
        super(entry.raw, entry.time, entry.types, entry.tag);
    }
}
exports.AreaChangeLogEntry = AreaChangeLogEntry;
