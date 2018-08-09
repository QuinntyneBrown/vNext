import { Component, Input, Output, EventEmitter } from "@angular/core";
import { Subject } from "rxjs";
import { Dashboard } from "./dashboard.model";
import {
  FormGroup,
  FormControl,
  Validators
} from "@angular/forms";

@Component({
  templateUrl: "./dashboard-list-item.component.html",
  styleUrls: ["./dashboard-list-item.component.css"],
  selector: "cs-dashboard-list-item"
})
export class DashboardListItemComponent { 

  public onDestroy: Subject<void> = new Subject<void>();

  ngOnDestroy() {
    this.onDestroy.next();	
  }

  ngOnInit() {
    this.form.patchValue({ "code": this.dashboard.code });

    this.form.valueChanges.subscribe(val => {
      const oldDashboard = Object.assign({}, this.dashboard);
      this.dashboard.code = val.code;

      this.onValueChanges.emit({ oldDashboard, newDashboard: this.dashboard });
    });
  }

  @Output()
  public onValueChanges: EventEmitter<any> = new EventEmitter();

  @Input()
  public dashboard: Dashboard = <Dashboard>{};

  @Output()
  public onDelete: EventEmitter<any> = new EventEmitter();

  public form = new FormGroup({
    code: new FormControl(this.dashboard.code)
  });

}
