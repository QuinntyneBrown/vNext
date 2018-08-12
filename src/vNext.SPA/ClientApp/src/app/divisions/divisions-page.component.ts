import { Component } from "@angular/core";
import { Subject } from "rxjs";

@Component({
  templateUrl: "./divisions-page.component.html",
  styleUrls: ["./divisions-page.component.css"],
  selector: "app-divisions-page"
})
export class DivisionsPageComponent { 

  public onDestroy: Subject<void> = new Subject<void>();

  ngOnDestroy() {
    this.onDestroy.next();	
  }
}
