import { Component, Input, OnInit } from "@angular/core";
import { Subject } from "rxjs";
import { Address } from "./address.model";
import { AddressConverter } from "./address-converter";

@Component({
  templateUrl: "./address.component.html",
  styleUrls: ["./address.component.css"],
  selector: "cs-address"
})
export class AddressComponent { 
  constructor(private _addressConvert: AddressConverter) { }
  
  private _address: Address = <Address>{};
  
  public get address(): Address { return this._address; }

  @Input()
  public set address(value: Address) {
    this._address = this._addressConvert.convert(value);
  }
}
