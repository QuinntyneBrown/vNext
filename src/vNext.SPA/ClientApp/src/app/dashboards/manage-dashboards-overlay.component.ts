import { Component } from "@angular/core";
import { Subject } from "rxjs";
import { ManageDashboardsOverlayRef } from "./manage-dashboards-overlay-ref";
import { DashboardStore } from "./dashboard-store";
import { DashboardService } from "./dashboard.service";
import { Dashboard } from "./dashboard.model";
import { LocalStorageService as Storage } from "../core/local-storage.service";
import { Observable } from "rxjs";
import { tap, flatMap } from "rxjs/operators";

@Component({
  templateUrl: "./manage-dashboards-overlay.component.html",
  styleUrls: ["./manage-dashboards-overlay.component.css"],
  selector: "cs-manage-dashboards-overlay",
  host: { '[class.mat-elevation-z6]':'true'}
})
export class ManageDashboardsOverlayComponent { 
  constructor(
    private _dashboardService: DashboardService,
    private _dashboardStore: DashboardStore,
    private _manageDashboardsOverlayRef: ManageDashboardsOverlayRef,
    private _storage: Storage,
  ) { }

  ngOnInit() {
    this.dashboards = JSON.parse(JSON.stringify(this._dashboardStore.dashboards$.value));
  }

  public onDestroy: Subject<void> = new Subject<void>();

  public cancel() {
    this._manageDashboardsOverlayRef.close();
  }

  public save() {
    this._manageDashboardsOverlayRef.close();
  }

  ngOnDestroy() {
    this.onDestroy.next();	
  }

  public handleCreate($event) {
    this.dashboards.push($event);
  }

  public tryToSaveDashboards() {
    //var observables :Array<Observable<any>> = [];
    //var currentDashboards = this._dashboardStore.dashboards$.value;
    //var newDashboards = this.dashboards;

    //for (var i = 0; i < currentDashboards.length; i++) {
    //  if (!newDashboards.find(x => x.dashboardId == currentDashboards[i].dashboardId)) {
    //    observables.push(this._dashboardService.remove({ dashboard: currentDashboards[i] }))
    //  }
    //}

    //for (var i = 0; i < newDashboards.length; i++) {
    //  var existingDashboard = currentDashboards.find(x => x.dashboardId == newDashboards[i].dashboardId);

    //  if ((existingDashboard && existingDashboard.code != newDashboards[i].code) || !existingDashboard) {
    //    var entity = Object.assign(newDashboards[i], { userId: this._storage.get({ name: constants.USER_ID_KEY }) });
    //    observables.push(this._dashboardService.save({ dashboard: entity }));
    //  }
    //}

    //forkJoin(observables)
    //  .pipe(
    //    flatMap(() => this._dashboardService.list()),
    //    tap((x: Array<Dashboard>) => this._dashboardStore.dashboards$.next(x)),
    //    tap(() => this._manageDashboardsOverlayRef.close())
    //  ).subscribe();    
  }

  public dashboards: Array<Dashboard> = [];

}
