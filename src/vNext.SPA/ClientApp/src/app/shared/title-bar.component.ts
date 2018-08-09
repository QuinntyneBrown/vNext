import { Component } from "@angular/core";
import { Subject } from "rxjs";

@Component({
  templateUrl: "./title-bar.component.html",
  styleUrls: ["./title-bar.component.css"],
  selector: "cs-title-bar"
})
export class TitleBarComponent { 

  public onDestroy: Subject<void> = new Subject<void>();

  ngOnDestroy() {
    this.onDestroy.next();	
  }
}
