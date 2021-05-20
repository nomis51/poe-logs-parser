"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.TradeParser = void 0;
const LogEntryType_1 = require("../enums/LogEntryType");
const TradeLogEntry_1 = require("../models/entries/TradeLogEntry");
const Item_1 = require("../models/Item");
const Price_1 = require("../models/Price");
const CurrencyService_1 = require("../services/CurrencyService");
class TradeParser {
    constructor(type, regMatch, regsClean) {
        this._type = type;
        this._regMatch = regMatch;
        this._regsClean = regsClean;
    }
    canParse(line) {
        return this._regMatch.test(line);
    }
    parse(line, entry) {
        let tradeLogEntry = new TradeLogEntry_1.TradeLogEntry(entry);
        tradeLogEntry.types = [
            LogEntryType_1.LogEntryType.Trade,
            LogEntryType_1.LogEntryType.Whisper,
            this._type
        ];
        const parts = line.split("#").filter(e => !!e && e !== "");
        tradeLogEntry.player = parts[0];
        const itemMatch = /[0-9]+/gi.exec(parts[1]);
        if (itemMatch && itemMatch.index === 0 && itemMatch.length === parts[1].length) {
            const itemParts = parts[1].split(" ");
            tradeLogEntry.item = new Item_1.Item();
            tradeLogEntry.item.quantity = Number(itemParts[0]);
            tradeLogEntry.item.name = itemParts.slice(1).reduce((prev, value) => `${prev} ${value}`, '');
        }
        else {
            tradeLogEntry.item = new Item_1.Item();
            tradeLogEntry.item.name = parts[1];
        }
        const priceParts = parts[2].split(" ");
        tradeLogEntry.price = new Price_1.Price();
        tradeLogEntry.price.value = Number(priceParts[0]);
        tradeLogEntry.price.currency = CurrencyService_1.getRealName(priceParts[1]);
        tradeLogEntry.price.NormalizedCurrency = priceParts[1].toLowerCase();
        tradeLogEntry.price.imageLink = CurrencyService_1.getCurrencyImageLink(priceParts[1].toLowerCase());
        tradeLogEntry.league = parts[3];
        if (parts.length < 4) {
            tradeLogEntry.location = {
                stashTab: parts[4],
                left: Number(parts[5]),
                top: Number(parts[6].endsWith(")") ? parts[6].substring(0, parts[6].length - 1) : parts[6])
            };
        }
        return tradeLogEntry;
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
exports.TradeParser = TradeParser;
