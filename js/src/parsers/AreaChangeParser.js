"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.AreaChangeParser = void 0;
const LogEntryType_1 = require("../enums/LogEntryType");
const AreaChangeLogEntry_1 = require("../models/entries/AreaChangeLogEntry");
class AreaChangeParser {
    constructor() {
        this._regMatch = /You have entered .+\\./gi;
        this._regsClean = [
            /: /gi,
            /You have entered /gi,
            /\\./gi
        ];
    }
    execute(entry, line) {
        const cleanedline = this.clean(line);
        return this.parse(cleanedline, entry);
    }
    canParse(line) {
        return this._regMatch.test(line);
    }
    parse(line, entry) {
        let areaChangeEntry = new AreaChangeLogEntry_1.AreaChangeLogEntry(entry);
        areaChangeEntry.types = [LogEntryType_1.LogEntryType.System, LogEntryType_1.LogEntryType.ChangeArea];
        areaChangeEntry.area = line;
        return areaChangeEntry;
    }
    clean(line) {
        this._regsClean.forEach((reg) => line = line.replace(reg, ""));
        return line;
    }
}
exports.AreaChangeParser = AreaChangeParser;
