import { Component } from "@angular/core";
import { Subject } from "rxjs";

@Component({
  templateUrl: "./create-division-overlay.component.html",
  styleUrls: ["./create-division-overlay.component.css"],
  selector: "app-create-division-overlay"
})
export class CreateDivisionOverlayComponent { 

  public onDestroy: Subject<void> = new Subject<void>();

  ngOnDestroy() {
    this.onDestroy.next();	
  }
}
