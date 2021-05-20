"use strict";
var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
Object.defineProperty(exports, "__esModule", { value: true });
exports.LogReaderService = void 0;
const findProcess = require('find-process');
const fs_1 = require("fs");
const os_1 = require("os");
const rxjs_1 = require("rxjs");
class LogReaderService {
    constructor(logFilePath = undefined) {
        this._poeProcesses = [
            "PathOfExile_x64",
            "PathOfExile_x64Steam"
        ];
        this._endOfFile = 0;
        this.newLogEntry = new rxjs_1.Subject();
        if (logFilePath) {
            this._logFilePath = logFilePath;
        }
        else {
            this.findLogFilePath();
        }
        this.setEndOfFile();
        this.watch();
    }
    watch() {
        return new Promise(() => __awaiter(this, void 0, void 0, function* () {
            while (true) {
                let newLines = [];
                do {
                    newLines = yield this.readNewLines();
                } while (newLines.length === 0);
                for (let line of newLines) {
                    this.newLogEntry.next(line);
                }
            }
        }));
    }
    readNewLines() {
        return new Promise((resolve) => {
            let lines = [];
            let currentPosition = this._endOfFile;
            this.setEndOfFile();
            if (currentPosition >= this._endOfFile) {
                return resolve(lines);
            }
            fs_1.stat(this._logFilePath, (err, fileInfos) => {
                if (err) {
                    console.error(err);
                    return resolve(lines);
                }
                fs_1.open(this._logFilePath, "r", (err, file) => {
                    if (err) {
                        console.error(err);
                        return resolve(lines);
                    }
                    let buffer;
                    try {
                        buffer = Buffer.alloc(fileInfos.size - this._endOfFile);
                    }
                    catch (e) {
                        console.error(e);
                        return resolve(lines);
                    }
                    fs_1.read(file, buffer, 0, buffer.length, currentPosition, (err, _, buffer) => {
                        if (err) {
                            console.error(err);
                            return resolve(lines);
                        }
                        let linesStr = "";
                        try {
                            linesStr = buffer.toString();
                        }
                        catch (e) {
                            console.error(e);
                            return resolve(lines);
                        }
                        return resolve(linesStr.split(os_1.EOL));
                    });
                });
            });
        });
    }
    removeSpecialChars(str) {
        return str ? str.replace(/[\\r\\n]/gi, "") : str;
    }
    setEndOfFile() {
        return Promise.resolve(fs_1.stat(this._logFilePath, (err, infos) => {
            if (err) {
                console.info("Cannot get Client.txt stats.");
                return;
            }
            this._endOfFile = infos.size;
        }));
    }
    findLogFilePath() {
        return __awaiter(this, void 0, void 0, function* () {
            for (var name of this._poeProcesses) {
                var processes = yield findProcess('name', name);
                if (processes && processes.length > 0) {
                    const process = processes[0];
                    const bin = process.bin;
                    if (os_1.platform() === 'win32') {
                        this._logFilePath = bin.substring(0, bin.lastIndexOf("\\"));
                        this._logFilePath += "\\logs\\Client.txt";
                    }
                    else {
                        this._logFilePath = bin.substring(0, bin.lastIndexOf("/"));
                        this._logFilePath += "/logs/Client.txt";
                    }
                }
            }
        });
    }
}
exports.LogReaderService = LogReaderService;
