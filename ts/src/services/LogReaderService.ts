const findProcess = require('find-process');
import { stat, open, read } from 'fs';
import { platform, EOL } from 'os'
import { Subject } from 'rxjs'

export class LogReaderService {
	private _poeProcesses: string[] = [
		"PathOfExile_x64",
		"PathOfExile_x64Steam"
	];
	private _logFilePath!: string;
	private _endOfFile: number = 0;

	public newLogEntry: Subject<string>;

	constructor(logFilePath: string | undefined = undefined) {
		this.newLogEntry = new Subject<string>();

		if (logFilePath) {
			this._logFilePath = logFilePath;
		} else {
			this.findLogFilePath();
		}

		this.setEndOfFile();
		this.watch();
	}

	private watch(): Promise<void> {
		return new Promise(async () => {
			while (true) {
				let newLines: string[] = [];

				do {
					newLines = await this.readNewLines();
				} while (newLines.length === 0);

				for (let line of newLines) {
					this.newLogEntry.next(line);
				}
			}
		});
	}

	private readNewLines(): Promise<string[]> {
		return new Promise((resolve) => {
			let lines: string[] = [];
			let currentPosition = this._endOfFile;
			this.setEndOfFile();

			if (currentPosition >= this._endOfFile) {
				return resolve(lines);
			}

			stat(this._logFilePath, (err, fileInfos) => {
				if (err) {
					console.error(err);
					return resolve(lines);
				}

				open(this._logFilePath, "r", (err, file) => {
					if (err) {
						console.error(err);
						return resolve(lines);
					}

					let buffer: Buffer;

					try {
						buffer = Buffer.alloc(fileInfos.size - this._endOfFile);
					} catch (e) {
						console.error(e);
						return resolve(lines);
					}

					read(
						file,
						buffer,
						0,
						buffer.length,
						currentPosition,
						(err, _, buffer) => {
							if (err) {
								console.error(err);
								return resolve(lines);
							}

							let linesStr: string = "";

							try {
								linesStr = buffer.toString();
							} catch (e) {
								console.error(e);
								return resolve(lines);
							}

							return resolve(linesStr.split(EOL));
						});
				});
			});
		});
	}

	private removeSpecialChars(str: string): string {
		return str ? str.replace(/[\\r\\n]/gi, "") : str;
	}

	private setEndOfFile(): Promise<void> {
		return Promise.resolve(stat(this._logFilePath, (err, infos) => {
			if (err) {
				console.info("Cannot get Client.txt stats.");
				return;
			}

			this._endOfFile = infos.size;
		}));
	}

	private async findLogFilePath(): Promise<void> {
		for (var name of this._poeProcesses) {
			var processes = await findProcess('name', name);

			if (processes && processes.length > 0) {
				const process = processes[0];
				const bin: string = (process as any).bin;

				if (platform() === 'win32') {
					this._logFilePath = bin.substring(0, bin.lastIndexOf("\\"));
					this._logFilePath += "\\logs\\Client.txt";
				} else {
					this._logFilePath = bin.substring(0, bin.lastIndexOf("/"));
					this._logFilePath += "/logs/Client.txt";
				}
			}
		}
	}
}