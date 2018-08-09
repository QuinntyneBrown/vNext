import { Component } from "@angular/core";
import { Subject } from "rxjs";
import { AddressService } from "./address.service";
import { AddressStore } from "./address-store";

@Component({
  templateUrl: "./address-page.component.html",
  styleUrls: ["./address-page.component.css"],
  selector: "cs-address-page"
})
export class AddressPageComponent { 
  constructor(
    private _addressService: AddressService,
    public addressStore: AddressStore,
  ) {
    
  }

  public onDestroy: Subject<void> = new Subject<void>();

  ngOnDestroy() {
    this.onDestroy.next();	
  }
}
