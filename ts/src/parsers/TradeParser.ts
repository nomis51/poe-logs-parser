import { LogEntryType } from "../enums/LogEntryType";
import { LogEntry } from "../models/entries/LogEntry";
import { TradeLogEntry } from "../models/entries/TradeLogEntry";
import { Item } from "../models/Item";
import { Price } from "../models/Price";
import { IParser } from "./abstractions/IParser";
import { StashLocation } from '../models/StashLocation';
import { getRealName, getCurrencyImageLink } from '../services/CurrencyService';

export class TradeParser implements IParser {
	private _regMatch!: RegExp;
	private _regsClean!: RegExp[];
	private _type: LogEntryType;

	constructor(type: LogEntryType, regMatch: RegExp, regsClean: RegExp[]) {
		this._type = type;
		this._regMatch = regMatch;
		this._regsClean = regsClean;
	}

	canParse(line: string): boolean {
		return this._regMatch.test(line);
	}

	parse(line: string, entry: LogEntry): TradeLogEntry {
		let tradeLogEntry: TradeLogEntry = new TradeLogEntry(entry);

		tradeLogEntry.types = [
			LogEntryType.Trade,
			LogEntryType.Whisper,
			this._type
		];

		const parts = line.split("#").filter(e => !!e && e !== "");
		tradeLogEntry.player = parts[0];

		const itemMatch = /[0-9]+/gi.exec(parts[1]);

		if (itemMatch && itemMatch.index === 0 && itemMatch.length === parts[1].length) {
			const itemParts = parts[1].split(" ");
			tradeLogEntry.item = new Item();
			tradeLogEntry.item.quantity = Number(itemParts[0]);
			tradeLogEntry.item.name = itemParts.slice(1).reduce((prev, value) => `${prev} ${value}`, '');
		} else {
			tradeLogEntry.item = new Item();
			tradeLogEntry.item.name = parts[1];
		}

		const priceParts = parts[2].split(" ");

		tradeLogEntry.price = new Price();
		tradeLogEntry.price.value = Number(priceParts[0]);
		tradeLogEntry.price.currency = getRealName(priceParts[1]);
		tradeLogEntry.price.NormalizedCurrency = priceParts[1].toLowerCase();
		tradeLogEntry.price.imageLink = getCurrencyImageLink(priceParts[1].toLowerCase());

		tradeLogEntry.league = parts[3];

		if (parts.length < 4) {
			tradeLogEntry.location = {
				stashTab: parts[4],
				left: Number(parts[5]),
				top: Number(parts[6].endsWith(")") ? parts[6].substring(0, parts[6].length - 1) : parts[6])
			} as StashLocation;
		}

		return tradeLogEntry;
	}

	clean(line: string): String {
		this._regsClean.forEach((reg) => line = line.replace(reg, ""));
		return line;
	}

	execute(entry: LogEntry, line: string): LogEntry {
		const cleanedline = this.clean(line);
		return this.parse(cleanedline as string, entry);
	}
}