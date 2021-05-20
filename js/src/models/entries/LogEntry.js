"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.LogEntry = void 0;
class LogEntry {
    constructor(raw, time, types, tag) {
        this.raw = raw;
        this.time = time;
        this.types = types;
        this.tag = tag;
    }
}
exports.LogEntry = LogEntry;
;
