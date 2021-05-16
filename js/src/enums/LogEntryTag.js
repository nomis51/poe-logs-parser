"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.ConvertLogEntryTag = exports.LogEntryTag = void 0;
var LogEntryTag;
(function (LogEntryTag) {
    LogEntryTag[LogEntryTag["Warn"] = 0] = "Warn";
    LogEntryTag[LogEntryTag["Info"] = 1] = "Info";
    LogEntryTag[LogEntryTag["Debug"] = 2] = "Debug";
})(LogEntryTag = exports.LogEntryTag || (exports.LogEntryTag = {}));
function ConvertLogEntryTag(str) {
    switch (str.toLowerCase()) {
        case 'warn':
            return LogEntryTag.Warn;
        case 'debug':
            return LogEntryTag.Debug;
        default:
            return LogEntryTag.Info;
    }
}
exports.ConvertLogEntryTag = ConvertLogEntryTag;
