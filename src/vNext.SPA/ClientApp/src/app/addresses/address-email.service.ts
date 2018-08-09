import { Injectable, Inject } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";
import { map } from "rxjs/operators";
import { baseUrl } from "../core/constants";
import { AddressEmail } from "./address-email.model";

@Injectable()
export class AddressEmailService {
  constructor(
    @Inject(baseUrl) private _baseUrl:string,
    private _client: HttpClient
  ) { }

  public get(): Observable<Array<AddressEmail>> {
    return this._client.get<{ addressEmails: Array<AddressEmail> }>(`${this._baseUrl}api/addressEmails`)
      .pipe(
        map(x => x.addressEmails)
      );
  }

  public getById(options: { addressEmailId: string }): Observable<AddressEmail> {
    return this._client.get<{ addressEmail: AddressEmail }>(`${this._baseUrl}api/addressEmails/${options.addressEmailId}`)
      .pipe(
        map(x => x.addressEmail)
      );
  }

  public remove(options: { addressEmail: AddressEmail }): Observable<void> {
    return this._client.delete<void>(`${this._baseUrl}api/addressEmails/${options.addressEmail.addressEmailId}`);
  }

  public create(options: { addressEmail: AddressEmail }): Observable<{ addressEmailId: string }> {
    return this._client.post<{ addressEmailId: string }>(`${this._baseUrl}api/addressEmails`, { addressEmail: options.addressEmail });
  }

  public update(options: { addressEmail: AddressEmail }): Observable<{ addressEmailId: string }> {
    return this._client.put<{ addressEmailId: string }>(`${this._baseUrl}api/addressEmails`, { addressEmail: options.addressEmail });
  }
}
