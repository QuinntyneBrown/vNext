import { Component } from "@angular/core";
import { Subject } from "rxjs";

@Component({
  templateUrl: "./territories-page.component.html",
  styleUrls: ["./territories-page.component.css"],
  selector: "app-territories-page"
})
export class TerritoriesPageComponent { 

  public onDestroy: Subject<void> = new Subject<void>();

  ngOnDestroy() {
    this.onDestroy.next();	
  }
}
