"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.PlayerJoinedAreaParser = void 0;
const LogEntryType_1 = require("../enums/LogEntryType");
const PlayerJoinedAreaLogEntry_1 = require("../models/entries/PlayerJoinedAreaLogEntry");
class PlayerJoinedAreaParser {
    constructor() {
        this._regMatch = /.+ has joined the area./gi;
        this._regsClean = [
            /: /gi,
            / has joined the area./gi
        ];
    }
    canParse(line) {
        return this._regMatch.test(line);
    }
    parse(line, entry) {
        let playerJoinedAreaLogEntry = new PlayerJoinedAreaLogEntry_1.PlayerJoinedAreaLogEntry(entry);
        playerJoinedAreaLogEntry.types = [LogEntryType_1.LogEntryType.System, LogEntryType_1.LogEntryType.Party, LogEntryType_1.LogEntryType.JoinArea];
        playerJoinedAreaLogEntry.player = line;
        return playerJoinedAreaLogEntry;
    }
    clean(line) {
        this._regsClean.forEach((reg) => line = line.replace(reg, ""));
        return line;
    }
    execute(entry, line) {
        const cleanedline = this.clean(line);
        return this.parse(cleanedline, entry);
    }
}
exports.PlayerJoinedAreaParser = PlayerJoinedAreaParser;
