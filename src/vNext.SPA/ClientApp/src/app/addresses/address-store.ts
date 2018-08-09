import { Injectable } from "@angular/core";
import { BehaviorSubject } from "rxjs";
import { AddressEmailType } from "./address-email-type.model";
import { AddressPhoneType } from "./address-phone-type.model";
import { Country } from "./country.model";
import { CountrySubdivision } from "./country-subdivision.model";

@Injectable()
export class AddressStore {
  constructor() {
    this.addressEmailTypes$ = new BehaviorSubject([]);
    this.addressPhoneTypes$ = new BehaviorSubject([]);
    this.countrySubdivisions$ = new BehaviorSubject([]);
    this.countries$ = new BehaviorSubject([]);
  }

  public addressEmailTypes$: BehaviorSubject<Array<AddressEmailType>>;

  public addressPhoneTypes$: BehaviorSubject<Array<AddressPhoneType>>;

  public countrySubdivisions$: BehaviorSubject<Array<CountrySubdivision>>;

  public countries$: BehaviorSubject<Array<Country>>;
}
