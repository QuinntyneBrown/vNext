import { Component } from "@angular/core";
import { Subject } from "rxjs";

@Component({
  templateUrl: "./sub-title-bar.component.html",
  styleUrls: ["./sub-title-bar.component.css"],
  selector: "cs-sub-title-bar"
})
export class SubTitleBarComponent { 

  public onDestroy: Subject<void> = new Subject<void>();

  ngOnDestroy() {
    this.onDestroy.next();	
  }
}
