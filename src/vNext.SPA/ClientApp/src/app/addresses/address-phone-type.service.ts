import { Injectable, Inject } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";
import { map } from "rxjs/operators";
import { baseUrl } from "../core/constants";
import { AddressPhoneType } from "./address-phone-type.model";

@Injectable()
export class AddressPhoneTypeService {
  constructor(
    @Inject(baseUrl) private _baseUrl:string,
    private _client: HttpClient
  ) { }

  public get(): Observable<Array<AddressPhoneType>> {
    return this._client.get<{ addressPhoneTypes: Array<AddressPhoneType> }>(`${this._baseUrl}api/addressPhoneTypes`)
      .pipe(
        map(x => x.addressPhoneTypes)
      );
  }

  public getById(options: { addressPhoneTypeId: string }): Observable<AddressPhoneType> {
    return this._client.get<{ addressPhoneType: AddressPhoneType }>(`${this._baseUrl}api/addressPhoneTypes/${options.addressPhoneTypeId}`)
      .pipe(
        map(x => x.addressPhoneType)
      );
  }

  public remove(options: { addressPhoneType: AddressPhoneType }): Observable<void> {
    return this._client.delete<void>(`${this._baseUrl}api/addressPhoneTypes/${options.addressPhoneType.addressPhoneTypeId}`);
  }

  public create(options: { addressPhoneType: AddressPhoneType }): Observable<{ addressPhoneTypeId: string }> {
    return this._client.post<{ addressPhoneTypeId: string }>(`${this._baseUrl}api/addressPhoneTypes`, { addressPhoneType: options.addressPhoneType });
  }

  public update(options: { addressPhoneType: AddressPhoneType }): Observable<{ addressPhoneTypeId: string }> {
    return this._client.put<{ addressPhoneTypeId: string }>(`${this._baseUrl}api/addressPhoneTypes`, { addressPhoneType: options.addressPhoneType });
  }
}
