"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.LogParserService = void 0;
const AreaChangeParser_1 = require("../parsers/AreaChangeParser");
const PlayerJoinedAreaParser_1 = require("../parsers/PlayerJoinedAreaParser");
const GlobalMessageParser_1 = require("../parsers/GlobalMessageParser");
const IncomingTradeParser_1 = require("../parsers/IncomingTradeParser");
const OutgoingTradeParser_1 = require("../parsers/OutgoingTradeParser");
const LogEntry_1 = require("../models/entries/LogEntry");
const LogEntryTag_1 = require("../enums/LogEntryTag");
const LogEntryType_1 = require("../enums/LogEntryType");
class LogParserService {
    constructor() {
        this._regLogEntryTag = /\[(debug|warn|info) Client [0-9]+\] /gi;
        this._parsers = [
            new AreaChangeParser_1.AreaChangeParser(),
            new PlayerJoinedAreaParser_1.PlayerJoinedAreaParser(),
            new GlobalMessageParser_1.GlobalMessageParser(),
            new IncomingTradeParser_1.IncomingTradeParser(),
            new OutgoingTradeParser_1.OutgoingTradeParser()
        ];
    }
    parse(line) {
        let entry = this.parseLogEntry(line);
        if (!entry)
            return;
        for (let parser of this._parsers.filter(p => p.canParse(line))) {
            try {
                return parser.execute(entry.entry, entry.line);
            }
            catch (_a) { }
        }
        entry.entry.types = [LogEntryType_1.LogEntryType.System];
        return entry.entry;
    }
    addParser(parser) {
        this._parsers.push(parser);
    }
    removeAllParsers() {
        this._parsers = [];
    }
    parseLogEntry(line) {
        var entry = new LogEntry_1.LogEntry(line, new Date(), [], LogEntryTag_1.LogEntryTag.Info);
        var dateEndIndex = line.indexOf(" ");
        if (dateEndIndex == -1) {
            return;
        }
        var timeEndIndex = line.indexOf(" ", dateEndIndex);
        if (timeEndIndex == -1) {
            return;
        }
        var dateTimeEndIndex = (dateEndIndex + timeEndIndex) - 1;
        var timeStr = line.substring(0, dateTimeEndIndex);
        line = line.substring((dateTimeEndIndex + 1));
        var logTagMatch = this._regLogEntryTag.exec(line);
        if (!logTagMatch) {
            return;
        }
        var tagStr = line.substring(logTagMatch.index + 1, line.indexOf(" ", logTagMatch.index));
        tagStr = tagStr.substring(0, tagStr.indexOf(" "));
        line = line.substring(logTagMatch.index + logTagMatch.length);
        entry.time = new Date(timeStr);
        entry.tag = LogEntryTag_1.ConvertLogEntryTag(tagStr);
        return { entry, line };
    }
}
exports.LogParserService = LogParserService;
