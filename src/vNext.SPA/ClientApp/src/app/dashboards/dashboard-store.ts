import { Injectable } from "@angular/core";
import { Dashboard } from "./dashboard.model";
import { Subject, BehaviorSubject } from "rxjs";

@Injectable()
export class DashboardStore {
  public dashboards$: BehaviorSubject<Array<Dashboard>> = new BehaviorSubject(null);
  public currentDashboard$: BehaviorSubject<Dashboard> = new BehaviorSubject(null);
}
