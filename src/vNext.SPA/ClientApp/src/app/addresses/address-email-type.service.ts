import { Injectable, Inject } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";
import { map } from "rxjs/operators";
import { baseUrl } from "../core/constants";
import { AddressEmailType } from "./address-email-type.model";

@Injectable()
export class AddressEmailTypeService {
  constructor(
    @Inject(baseUrl) private _baseUrl:string,
    private _client: HttpClient
  ) { }

  public get(): Observable<Array<AddressEmailType>> {
    return this._client.get<{ addressEmailTypes: Array<AddressEmailType> }>(`${this._baseUrl}api/addressEmailTypes`)
      .pipe(
        map(x => x.addressEmailTypes)
      );
  }

  public getById(options: { addressEmailTypeId: string }): Observable<AddressEmailType> {
    return this._client.get<{ addressEmailType: AddressEmailType }>(`${this._baseUrl}api/addressEmailTypes/${options.addressEmailTypeId}`)
      .pipe(
        map(x => x.addressEmailType)
      );
  }

  public remove(options: { addressEmailType: AddressEmailType }): Observable<void> {
    return this._client.delete<void>(`${this._baseUrl}api/addressEmailTypes/${options.addressEmailType.addressEmailTypeId}`);
  }

  public create(options: { addressEmailType: AddressEmailType }): Observable<{ addressEmailTypeId: string }> {
    return this._client.post<{ addressEmailTypeId: string }>(`${this._baseUrl}api/addressEmailTypes`, { addressEmailType: options.addressEmailType });
  }

  public update(options: { addressEmailType: AddressEmailType }): Observable<{ addressEmailTypeId: string }> {
    return this._client.put<{ addressEmailTypeId: string }>(`${this._baseUrl}api/addressEmailTypes`, { addressEmailType: options.addressEmailType });
  }
}
