import { Component, EventEmitter, Input, Output } from "@angular/core";
import { FormControl, FormGroup, Validators } from "@angular/forms";
import { BehaviorSubject, Subject } from "rxjs";
import { NotificationService } from "../core/notification.service";
import { AddressConverter } from "./address-converter";
import { AddressEmailType } from "./address-email-type.model";
import { AddressEmail } from "./address-email.model";
import { AddressPhoneType } from "./address-phone-type.model";
import { AddressPhone } from "./address-phone.model";
import { Address } from "./address.model";
import { AddressService } from "./address.service";
import { Country } from "./country.model";
import { CountrySubdivision } from "./country-subdivision.model";

@Component({
  templateUrl: "./address-editor.component.html",
  styleUrls: ["./address-editor.component.css"],
  selector: "cs-address-editor"
})
export class AddressEditorComponent {
  constructor(
    private _addressConvert: AddressConverter,
    private _addressService: AddressService,
    private _notificationService: NotificationService
  ) { }

  public onDestroy: Subject<void> = new Subject<void>();
  
  ngOnDestroy() {
    this.onDestroy.next();	
  }

  @Input()
  public addressId: number = 0;

  @Input()
  public countrySubdivisions: CountrySubdivision[] = [];

  @Input()
  public countries: Country[] = [];
  
  private _address: Address = new Address();

  @Input("address")
  public set address(value: Address) {
    this._address = value;
    console.log(this._address);

    this.form.patchValue({
      address: this._address.addressStreet,
      city: this._address.city,
      postalZipCode: this._address.postalZipCode,
      county: this._address.county,
      countrySubdivisionId: this._address.countrySubdivisionId,
      countryId: this._address.countryId,

      phone: this._address.phone,
      fax: this._address.fax,
      email: this._address.email,
      website: this._address.website,
    });
  }

  public get address() { return this._address; }
  
  public form = new FormGroup({
    address: new FormControl(this._address.addressStreet, [Validators.required, Validators.maxLength(256)]),
    city: new FormControl(this._address.city, [Validators.required, Validators.maxLength(50)]),
    postalZipCode: new FormControl(this._address.postalZipCode, [Validators.required, Validators.maxLength(20)]),
    county: new FormControl(this._address.county, [Validators.required, Validators.maxLength(50)]),
    countrySubdivisionId: new FormControl(this._address.countrySubdivisionId, [Validators.required]),
    countryId: new FormControl(this._address.countryId, [Validators.required]),

    phone: new FormControl(this._address.phone, [Validators.maxLength(30)]),
    fax: new FormControl(this._address.fax, [Validators.maxLength(30)]),
    email: new FormControl(this._address.email, [Validators.email, Validators.maxLength(30)]),
    website: new FormControl(this._address.website, [Validators.maxLength(256)]),
  });

  public tryToSave() {
    let address = new Address();
  
    address.addressStreet = this.form.value.address;
    address.city = this.form.value.city;
    address.postalZipCode = this.form.value.postalZipCode;
    address.county = this.form.value.county;
    address.countrySubdivisionId = this.form.value.countrySubdivisionId;
    address.phone = this.form.value.phone;
    address.fax = this.form.value.fax;
    address.email = this.form.value.email;
    address.website = this.form.value.website;
     
    this.save.emit({ entity: address });
  }

  @Output()
  public save: EventEmitter<any> = new EventEmitter();

  @Output()
  public cancel: EventEmitter<any> = new EventEmitter();

}
