import { AddressStore } from "./address-store";
import { Injectable } from "@angular/core";
import { Address } from "./address.model";
import { AddressPhoneType } from "./address-phone-type.model";
import { AddressEmailType } from "./address-email-type.model";

@Injectable()
export class AddressConverter {
  constructor(private _addressStore: AddressStore) { }

  public convert(value: Address) {
    if (!value) return value;

    var countrySubdivisions = this._addressStore.countrySubdivisions$.value;
    var countries = this._addressStore.countries$.value;
    
    for (let i = 0; i < countrySubdivisions.length; i++) {
      if (value.countrySubdivisionId == countrySubdivisions[i].countrySubdivisionId)
        value.countrySubdivision = countrySubdivisions[i];
    }

    for (let i = 0; i < countries.length; i++) {
      if (value.countrySubdivision.countryId == countries[i].countryId) {
        value.country = countries[i];
        value.countryId = countries[i].countryId;
      }
    }
    
    for (let i = 0; i < value.addressPhones.length; i++) {
      value.addressPhones[i].addressPhoneType = this._addressStore.addressPhoneTypes$.value.find(x => x.addressPhoneTypeId == value.addressPhones[i].addressPhoneTypeId);
    }

    for (let i = 0; i < value.addressEmails.length; i++) {
      value.addressEmails[i].addressEmailType = this._addressStore.addressEmailTypes$.value.find(x => x.addressEmailTypeId == value.addressEmails[i].addressEmailTypeId);      
    }

    return value;
  }
}
