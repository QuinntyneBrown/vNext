import { Component, Output, EventEmitter } from "@angular/core";
import { Subject, BehaviorSubject } from "rxjs";
import { FormGroup, FormControl } from "@angular/forms";
import { AddressService } from "./address.service";
import { Address } from "./address.model";
import { map, switchMap, tap, takeUntil } from "rxjs/operators";
import { OverlayRefWrapper } from "../core/overlay-ref-wrapper";
import { AddressStore } from "./address-store";
import { AddressConverter } from "./address-converter";

@Component({
  templateUrl: "./edit-address-side-bar.component.html",
  styleUrls: ["./edit-address-side-bar.component.css"],
  selector: "cs-edit-address-side-bar"
})
export class EditAddressSideBarComponent { 
  constructor(
    private _addressConvert: AddressConverter,
    private _addressService: AddressService,
    public addressStore: AddressStore
  ) { }

  ngOnInit() {
    if (this.addressId) {
      this._addressService.getById({ addressId: this.addressId})
        .pipe(
          takeUntil(this.onDestroy),
          map((x: Address) => {
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

  public handleSave($event) {
    this._addressService.save({ address: $event.entity })
      .pipe(tap(x => this.save.emit(x)), takeUntil(this.onDestroy))
      .subscribe();
  }

  @Output()
  public cancel: EventEmitter<any> = new EventEmitter();

  @Output()
  public save: EventEmitter<any> = new EventEmitter();
}
