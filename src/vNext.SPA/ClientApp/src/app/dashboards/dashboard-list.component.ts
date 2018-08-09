import { Component, Input, EventEmitter, Output } from "@angular/core";
import { Subject } from "rxjs";
import { Dashboard } from "./dashboard.model";

@Component({
  templateUrl: "./dashboard-list.component.html",
  styleUrls: ["./dashboard-list.component.css"],
  selector: "cs-dashboard-list"
})
export class DashboardListComponent { 

  public onDestroy: Subject<void> = new Subject<void>();

  ngOnDestroy() {
    this.onDestroy.next();	
  }

  @Input()
  public dashboards: Array<Dashboard> = [];

  public handleDelete(dashboard) {    
    this.dashboards.splice(this.dashboards.indexOf(dashboard), 1);
  }

  public handValueChange($event) {
    this.dashboards[this.dashboards.indexOf($event.oldDashboard)] = $event.newDashboard;
  }

  @Output()
  public onDelete: EventEmitter<any> = new EventEmitter();
}
