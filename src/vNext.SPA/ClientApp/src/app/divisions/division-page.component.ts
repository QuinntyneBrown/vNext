import { Component } from "@angular/core";
import { Subject } from "rxjs";

@Component({
  templateUrl: "./division-page.component.html",
  styleUrls: ["./division-page.component.css"],
  selector: "app-division-page"
})
export class DivisionPageComponent { 

  public onDestroy: Subject<void> = new Subject<void>();

  ngOnDestroy() {
    this.onDestroy.next();	
  }
}
