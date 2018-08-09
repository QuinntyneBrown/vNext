import { Component, EventEmitter, Output } from "@angular/core";
import { FormControl, FormGroup } from "@angular/forms";
import { Subject } from "rxjs";
import { Dashboard } from "./dashboard.model";

@Component({
  templateUrl: "./dashboard-edit.component.html",
  styleUrls: ["./dashboard-edit.component.css"],
  selector: "cs-dashboard-edit"
})
export class DashboardEditComponent { 

  public onDestroy: Subject<void> = new Subject<void>();

  ngOnDestroy() {
    this.onDestroy.next();	
  }

  code: string;

  public form = new FormGroup({
    code: new FormControl(this.code)
  });

  public tryToCreate($event) {
    this.onCreate.emit(new Dashboard(this.form.value.code));
    this.form.patchValue({ "code": null });
  }

  @Output()
  public onCreate: EventEmitter<any> = new EventEmitter();
}
