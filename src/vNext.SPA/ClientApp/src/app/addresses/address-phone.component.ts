import { Component } from "@angular/core";
import { Subject } from "rxjs";

@Component({
  templateUrl: "./address-phone.component.html",
  styleUrls: ["./address-phone.component.css"],
  selector: "cs-address-phone"
})
export class AddressPhoneComponent { 

  public onDestroy: Subject<void> = new Subject<void>();

  ngOnDestroy() {
    this.onDestroy.next();	
  }
}
