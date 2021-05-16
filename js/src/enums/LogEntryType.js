"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.LogEntryType = void 0;
var LogEntryType;
(function (LogEntryType) {
    LogEntryType[LogEntryType["Trade"] = 0] = "Trade";
    LogEntryType[LogEntryType["Global"] = 1] = "Global";
    LogEntryType[LogEntryType["Whisper"] = 2] = "Whisper";
    LogEntryType[LogEntryType["Party"] = 3] = "Party";
    LogEntryType[LogEntryType["Local"] = 4] = "Local";
    LogEntryType[LogEntryType["Incoming"] = 5] = "Incoming";
    LogEntryType[LogEntryType["Outgoing"] = 6] = "Outgoing";
    LogEntryType[LogEntryType["System"] = 7] = "System";
    LogEntryType[LogEntryType["ChatMessage"] = 8] = "ChatMessage";
    LogEntryType[LogEntryType["JoinArea"] = 9] = "JoinArea";
    LogEntryType[LogEntryType["LeaveArea"] = 10] = "LeaveArea";
    LogEntryType[LogEntryType["ChangeArea"] = 11] = "ChangeArea";
})(LogEntryType = exports.LogEntryType || (exports.LogEntryType = {}));
