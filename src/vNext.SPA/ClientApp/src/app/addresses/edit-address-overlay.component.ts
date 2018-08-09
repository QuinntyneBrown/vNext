import { Component } from "@angular/core";
import { Subject, BehaviorSubject } from "rxjs";
import { FormGroup, FormControl } from "@angular/forms";
import { AddressService } from "./address.service";
import { Address } from "./address.model";
import { map, switchMap, tap, takeUntil } from "rxjs/operators";
import { AddressStore } from "./address-store";
import { AddressConverter } from "./address-converter";
import { OverlayRefWrapper } from "../core/overlay-ref-wrapper";

@Component({
  templateUrl: "./edit-address-overlay.component.html",
  styleUrls: ["./edit-address-overlay.component.css"],
  selector: "cs-edit-address-overlay"
})
export class EditAddressOverlayComponent { 
  constructor(
    private _addressConvert: AddressConverter,
    private _addressService: AddressService,
    public addressStore: AddressStore,
    private _overlay: OverlayRefWrapper
  ) { }

  ngOnInit() {    
    if (this.addressId) {      
      this._addressService.getById({ addressId: this.addressId })
        .pipe(
          takeUntil(this.onDestroy),          
          map((x:Address) => {
            x = this._addressConvert.convert(x);
            this.address$.next(x);
          })
        )
        .subscribe();
    }
  }

  public onDestroy: Subject<void> = new Subject<void>();

  ngOnDestroy() {
    this.onDestroy.next();	
  }

  public address$: BehaviorSubject<Address> = new BehaviorSubject(new Address());
  
  public addressId: number;

  public cancel() {
    this._overlay.close();
  }

  public handleSave($event) {
    this._addressService.update({ address: $event.entity })
      .pipe(
        takeUntil(this.onDestroy),
        tap(x => this._overlay.close(x)))
      .subscribe();
  }
} 
