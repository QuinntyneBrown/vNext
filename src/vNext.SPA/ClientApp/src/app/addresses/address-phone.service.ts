import { Injectable, Inject } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";
import { map } from "rxjs/operators";
import { baseUrl } from "../core/constants";
import { AddressPhone } from "./address-phone.model";

@Injectable()
export class AddressPhoneService {
  constructor(
    @Inject(baseUrl) private _baseUrl:string,
    private _client: HttpClient
  ) { }

  public get(): Observable<Array<AddressPhone>> {
    return this._client.get<{ addressPhones: Array<AddressPhone> }>(`${this._baseUrl}api/addressPhones`)
      .pipe(
        map(x => x.addressPhones)
      );
  }

  public getById(options: { addressPhoneId: string }): Observable<AddressPhone> {
    return this._client.get<{ addressPhone: AddressPhone }>(`${this._baseUrl}api/addressPhones/${options.addressPhoneId}`)
      .pipe(
        map(x => x.addressPhone)
      );
  }

  public remove(options: { addressPhone: AddressPhone }): Observable<void> {
    return this._client.delete<void>(`${this._baseUrl}api/addressPhones/${options.addressPhone.addressPhoneId}`);
  }

  public create(options: { addressPhone: AddressPhone }): Observable<{ addressPhoneId: string }> {
    return this._client.post<{ addressPhoneId: string }>(`${this._baseUrl}api/addressPhones`, { addressPhone: options.addressPhone });
  }

  public update(options: { addressPhone: AddressPhone }): Observable<{ addressPhoneId: string }> {
    return this._client.put<{ addressPhoneId: string }>(`${this._baseUrl}api/addressPhones`, { addressPhone: options.addressPhone });
  }
}
