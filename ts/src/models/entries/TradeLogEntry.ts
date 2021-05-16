import { Item } from "../Item";
import { Price } from "../Price";
import { StashLocation } from "../StashLocation";
import { LogEntry } from "./LogEntry";

export class TradeLogEntry extends LogEntry {
	public player!: String;
	public item!: Item;
	public league!: String;
	public price!: Price;
	public location!: StashLocation;

	constructor(entry: LogEntry) {
		super(entry.raw as string, entry.time, entry.types, entry.tag);
	}
}