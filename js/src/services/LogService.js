"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.LogService = void 0;
const rxjs_1 = require("rxjs");
const LogParserService_1 = require("./LogParserService");
const LogReaderService_1 = require("./LogReaderService");
class LogService {
    constructor(logFilePath = undefined) {
        this.newLogEntry = new rxjs_1.Subject();
        this.newTradeLogEntry = new rxjs_1.Subject();
        this.newPlayerJoinedAreaLogEntry = new rxjs_1.Subject();
        this.newAreaChangeLogEntry = new rxjs_1.Subject();
        this.newChatMessageLogEntry = new rxjs_1.Subject();
        this._logParserService = new LogParserService_1.LogParserService();
        this._logReaderService = new LogReaderService_1.LogReaderService(logFilePath);
        this._logReaderService.newLogEntry.subscribe(this.onNewLogEntry);
    }
    onNewLogEntry(line) {
        if (!line)
            return;
        const entry = this._logParserService.parse(line);
        if (!entry)
            return;
        this.newLogEntry.next(entry);
    }
    addParser(parser) {
        this._logParserService.addParser(parser);
    }
    addParsers(parsers = []) {
        if (parsers.length === 0)
            return;
        for (const parser of parsers) {
            this._logParserService.addParser(parser);
        }
    }
    removeAllParsers() {
        this._logParserService.removeAllParsers();
    }
    parse(line) {
        return this._logParserService.parse(line);
    }
}
exports.LogService = LogService;
