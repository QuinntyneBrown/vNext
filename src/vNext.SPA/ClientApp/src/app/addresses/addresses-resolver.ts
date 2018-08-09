import { Injectable } from "@angular/core";
import { BehaviorSubject, Observable } from "rxjs";
import { AddressEmailType } from "./address-email-type.model";
import { AddressPhoneType } from "./address-phone-type.model";
import { Resolve, ActivatedRouteSnapshot, RouterStateSnapshot } from "@angular/router";
import { AddressEmailTypeService } from "./address-email-type.service";
import { AddressPhoneTypeService } from "./address-phone-type.service";
import { CountryService } from "./country.service";
import { combineLatest } from "rxjs";
import { map } from "rxjs/operators";
import { AddressStore } from "./address-store";
import { CountrySubdivisionService } from "./country-subdivision.service";


@Injectable()
export class AddressesResolver implements Resolve<boolean> {

  constructor(
    private _addressEmailTypeService: AddressEmailTypeService,
    private _addressPhoneTypeService: AddressPhoneTypeService,
    private _countrySubDivisionService: CountrySubdivisionService,
    private _countryService: CountryService,
    private _addressStore : AddressStore
  ) { }

  resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean> {
    const addressEmailTypes$ = this._addressEmailTypeService.get().pipe(map(x => this._addressStore.addressEmailTypes$.next(x)));
    const addressPhoneTypes$ = this._addressPhoneTypeService.get().pipe(map(x => this._addressStore.addressPhoneTypes$.next(x)));
    const countrySubDivision$ = this._countrySubDivisionService.get().pipe(map(x => this._addressStore.countrySubdivisions$.next(x)));
    const countries$ = this._countryService.get().pipe(map(x => this._addressStore.countries$.next(x)));

    return combineLatest([addressEmailTypes$, addressPhoneTypes$, countrySubDivision$, countries$])
      .pipe(
        map(x => true)
      ); 
  }
}
