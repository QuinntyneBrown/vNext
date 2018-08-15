import { Component, Input, Output, EventEmitter } from "@angular/core";
import { Subject } from "rxjs";
import { FormGroup, FormControl, Validators, ValidatorFn, ValidationErrors } from "@angular/forms";
import { Contact } from "./contact.model";
import { contactValidator } from "./contact.validator";
import { Address } from "../addresses/address.model";
import { hasAddressValidator } from "../addresses/has-address.validator";

@Component({
  templateUrl: "./contact-editor.component.html",
  styleUrls: ["./contact-editor.component.css"],
  selector: "cs-contact-editor"
})
export class ContactEditorComponent { 

  public onDestroy: Subject<void> = new Subject<void>();

  ngOnDestroy() {
    this.onDestroy.next();	
  }

  public _contact: Contact = new Contact();
  
  public form = new FormGroup({
    contactId: new FormControl(null, []),
    addressId: new FormControl(this._contact.addressId,[]),
    firstName: new FormControl(this._contact.firstName, [Validators.maxLength(128)]),
    middleName: new FormControl(this._contact.middleName, [Validators.maxLength(128)]),
    lastName: new FormControl(this._contact.lastName, [Validators.maxLength(128)]),
    companyName: new FormControl(this._contact.companyName, [Validators.maxLength(128)])
  }, { validators: [contactValidator, hasAddressValidator] });
  
  @Output()
  public save: EventEmitter<any> = new EventEmitter();

  @Output()
  public cancel: EventEmitter<any> = new EventEmitter();

  @Output()
  public editAddressClick: EventEmitter<any> = new EventEmitter();

  private _disabled: boolean = false;

  public get disabled() { return this._disabled; }

  @Input("disabled")
  public set disabled(value: boolean) {
    this._disabled = value;
    var _ = value ? this.form.disable() : this.form.enable();
  }

  @Input("address")
  public set address(value:Address) {
    if (value) {
      this._address = value;

      this.form.patchValue({
        addressId: value.addressId,
      });
    }
  }

  @Input("contact")
  public set contact(value: Contact) {
    console.log(value);
    if (value.address)
      this.address = value.address;

    this.form.patchValue({
      contactId: value.contactId,
      addressId: value.addressId,
      firstName: value.firstName,
      lastName: value.lastName,
      companyName: value.companyName,
      middleName: value.middleName
    });
  }

  private _address: Address;

  public get address() {
    return this._address;
  }
  
  public handleEditAddressClick() {
    this.editAddressClick.emit({ addressId: this._contact.addressId });
  }
}
