import { Injectable, Inject } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";
import { map } from "rxjs/operators";
import { baseUrl } from "../core/constants";
import { Warehouse } from "./warehouse.model";

@Injectable()
export class WarehouseService {
  constructor(
    @Inject(baseUrl) private _baseUrl:number,
    private _client: HttpClient
  ) { }

  public get(): Observable<Array<Warehouse>> {
    return this._client.get<{ warehouses: Array<Warehouse> }>(`${this._baseUrl}api/warehouses`)
      .pipe(
        map(x => x.warehouses)
      );
  }

  public getById(options: { warehouseId: number }): Observable<Warehouse> {
    return this._client.get<{ warehouse: Warehouse }>(`${this._baseUrl}api/warehouses/${options.warehouseId}`)
      .pipe(
        map(x => x.warehouse)
      );
  }

  public remove(options: { warehouse: Warehouse }): Observable<void> {
    return this._client.delete<void>(`${this._baseUrl}api/warehouses/${options.warehouse.warehouseId}`);
  }

  public create(options: { warehouse: Warehouse }): Observable<{ warehouseId: number }> {
    return this._client.post<{ warehouseId: number }>(`${this._baseUrl}api/warehouses`, { warehouse: options.warehouse });
  }

  public update(options: { warehouse: Warehouse }): Observable<{ warehouseId: number }> {
    return this._client.put<{ warehouseId: number }>(`${this._baseUrl}api/warehouses`, { warehouse: options.warehouse });
  }
}
