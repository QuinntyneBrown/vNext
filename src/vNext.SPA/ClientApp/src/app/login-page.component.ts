import { Component } from "@angular/core";
import { Subject } from "rxjs";
import { DashboardService } from "./dashboards/dashboard.service";
import { AuthService } from "./core/auth.service";
import { RedirectService } from "./core/redirect.service";
import { takeUntil, switchMap, tap } from "rxjs/operators";
import { LocalStorageService } from "./core/local-storage.service";
import { userIdKey, dashboardsKey } from "./core/constants";

@Component({
  templateUrl: "./login-page.component.html",
  styleUrls: ["./login-page.component.css"],
  selector: "app-login-page"
})
export class LoginPageComponent { 
  constructor(
    private _authService: AuthService,
    private _dashboardService: DashboardService,
    private _redirectService: RedirectService,
    private _storage: LocalStorageService
  ) { }

  public tryToLogin($event) {
    this._authService
      .tryToLogin({
        code: $event.value.username,
        password: $event.value.password,
        customerKey: 'QUINNTYNE_DEV'
      })
      .pipe(
        switchMap(() => this.tryToLoadDashboards()),
        takeUntil(this.onDestroy))
      .subscribe(
        () => this._redirectService.redirectPreLogin(),
        errorResponse => this.handleErrorResponse(errorResponse)
      );
  }

  public tryToLoadDashboards() {
    return this._dashboardService.getAllByUserId({ userId: this._storage.get({ name: userIdKey }) })
      .pipe(tap(x => {
        this._storage.put({ name: dashboardsKey, value: x });
      }));
  }

  public disabled: boolean = false;

  handleErrorResponse(error: any): any {
    throw new Error("Method not implemented.");
  }

  public onDestroy: Subject<void> = new Subject<void>();

  ngOnDestroy() {
    this.onDestroy.next();	
  }
}
