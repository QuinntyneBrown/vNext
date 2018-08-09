import { Component } from "@angular/core";
import { Subject } from "rxjs";

@Component({
  templateUrl: "./user-editor.component.html",
  styleUrls: ["./user-editor.component.css"],
  selector: "app-user-editor"
})
export class UserEditorComponent { 

  public onDestroy: Subject<void> = new Subject<void>();

  ngOnDestroy() {
    this.onDestroy.next();	
  }
}
