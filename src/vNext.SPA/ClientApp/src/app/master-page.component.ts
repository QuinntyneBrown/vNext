import { Component, HostBinding } from '@angular/core';
import { AuthService } from './core/auth.service';
import { NotificationService } from './core/notification.service';
import { map, tap, takeUntil } from 'rxjs/operators';
import { DashboardService } from './dashboards/dashboard.service';
import { LocalStorageService } from './core/local-storage.service';
import { DashboardStore } from './dashboards/dashboard-store';
import { userIdKey, dashboardsKey } from './core/constants';

@Component({
  templateUrl: './master-page.component.html',
  styleUrls: ['./master-page.component.css'],
  selector: 'app-master-page'
})
export class MasterPageComponent {
  constructor(
    private _authService: AuthService,
    private _dashboardService: DashboardService,
    private _dashboardStore: DashboardStore,
    private _localStorageService: LocalStorageService,
    public notificationService: NotificationService
  ) { }

  ngOnInit() {
    this._dashboardService.getAllByUserId({ userId: +this._localStorageService.get({ name: userIdKey }) })
      .pipe(tap(x => this._localStorageService.put({ name: dashboardsKey, value: x })))
      .subscribe();

    this.notificationService.errors$
      .pipe(tap(x => {
        if (x.length < 1) {
          this.isErrorConsoleOpen = false;
        }
        else {
          this.isErrorConsoleOpen = true;
        }
      }))
      .subscribe();    
  }
  
  public signOut() {
    this._authService.logout();
  }
  
  public clearNotifcations() {
    this.notificationService.errors$.next([]);
  }

  public closeErrorConsole() {
    this.isErrorConsoleOpen = false;
  }

  @HostBinding("class.error-console-is-opened")
  public isErrorConsoleOpen:boolean = null;
}
