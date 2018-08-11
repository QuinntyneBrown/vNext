import { Component } from "@angular/core";
import { Subject, Observable } from "rxjs";
import { WarehouseService } from "./warehouse.service";
import { Warehouse } from "./warehouse.model";

@Component({
  templateUrl: "./warehouses-page.component.html",
  styleUrls: ["./warehouses-page.component.css"],
  selector: "app-warehouses-page"
})
export class WarehousesPageComponent { 
  constructor(
    private readonly _warehouseService: WarehouseService) {
    this.warehouses$ = this._warehouseService.get();
  }

  ngOnInit() {
    
  }


  public onDestroy: Subject<void> = new Subject<void>();

  public columns: { field: string, header: string }[] = [
    {
      field: "warehouseId",
      header: "Id"
    },
    {
      field: "code",
      header: "Code"
    },
    {
      field: "description",
      header: "Description"
    },
    {
      field: "divisionId",
      header: "Division Id"
    },
    {
      field: "addressId",
      header: "Address Id"
    }

  ];

  ngOnDestroy() {
    this.onDestroy.next();	
  }

  public readonly warehouses$: Observable<Warehouse[]>;
}
