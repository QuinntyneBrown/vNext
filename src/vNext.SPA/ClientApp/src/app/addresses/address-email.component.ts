import { Component } from "@angular/core";
import { Subject } from "rxjs";

@Component({
  templateUrl: "./address-email.component.html",
  styleUrls: ["./address-email.component.css"],
  selector: "cs-address-email"
})
export class AddressEmailComponent { 

  public onDestroy: Subject<void> = new Subject<void>();

  ngOnDestroy() {
    this.onDestroy.next();	
  }
}
