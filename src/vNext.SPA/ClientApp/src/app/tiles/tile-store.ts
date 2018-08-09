import { Injectable } from "@angular/core";
import { BehaviorSubject } from "rxjs";
import { Tile } from "./tile.model";

@Injectable()
export class TileStore {
  constructor() {

  }

  public getTileNameByTileId(tileId: number) {
    return this.tiles$.value.find(x => x.tileId == tileId).code;
  }

  public tiles$: BehaviorSubject<Array<Tile>> = new BehaviorSubject([
    { tileId: 1, code: "Favorites", description: "List of Favorites" },
    { tileId: 2, code: "Activities", description: "List of Activities" },
    { tileId: 3, code: "Customers", description: "List of recent Customers" },
    { tileId: 4, code: "Suppliers", description: "List of recent Suppliers" },
    { tileId: 5, code: "Specifications", description: "List of recent Specifications" },
    { tileId: 6, code: "Estimates", description: "List of recent Estimates" },
    { tileId: 7, code: "Projects", description: "List of recent Projects" },
    { tileId: 8, code: "Quotes", description: "List of recent Quotes" },
    { tileId: 9, code: "Sales Orders", description: "List of recent Sales Orders" },
    { tileId: 10, code: "Purchase Orders", description: "List of recent Purchase Orders" },
    { tileId: 11, code: "Stock Requisitions", description: "List of recent Stock Requisitions" },
    { tileId: 12, code: "Work Orders", description: "List of recent Work Orders" },
    { tileId: 13, code: "Receivings", description: "List of recent Receivings" },
    { tileId: 14, code: "WIP Transfers", description: "List of recent WIP Transfers" },
    { tileId: 15, code: "Shipments", description: "List of recent Shipments" },
    { tileId: 16, code: "Customer Returns", description: "List of recent Customer Returns" },
    { tileId: 17, code: "Invoices", description: "List of recent Invoices" },
    { tileId: 18, code: "Application for Payments", description: "List of recent Application for Payments" },
    { tileId: 19, code: "Customer Payments", description: "List of recent Customer Payments" },
    { tileId: 20, code: "Payables", description: "List of recent Payables" },
    { tileId: 21, code: "Pool To Stock", description: "List of recent Pool To Stock" },
    { tileId: 22, code: "Pool To Garbage", description: "List of recent Pool To Garbage" },
    { tileId: 23, code: "Supplier Returns", description: "List of recent Supplier Returns" },
    { tileId: 24, code: "Check Run", description: "List of recent Check Runs" },
    { tileId: 25, code: "Product Counts", description: "List of recent Product Counts" },
    { tileId: 26, code: "Stock Transfers", description: "List of recent Stock Transfers" },
    { tileId: 27, code: "Cash Drawers", description: "List of recent Cash Drawers" },
    { tileId: 28, code: "Journal Entries", description: "List of recent Journal Entries" }

  ]);
}
