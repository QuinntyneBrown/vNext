import { Component } from "@angular/core";
import { Subject, BehaviorSubject } from "rxjs";
import { FormGroup, FormControl } from "@angular/forms";
import { OverlayRefWrapper } from "../core/overlay-ref-wrapper";
import { UserService } from "./user.service";
import { User } from "./user.model";
import { map, switchMap, tap, takeUntil } from "rxjs/operators";

@Component({
  templateUrl: "./create-user-overlay.component.html",
  styleUrls: ["./create-user-overlay.component.css"],
  selector: "app-create-user-overlay",
  host: { 'class': 'mat-typography' }
})
export class CreateUserOverlayComponent { 
  constructor(
    private _userService: UserService,
    private _overlay: OverlayRefWrapper) { }

  ngOnInit() {
    if (this.userId)
      this._userService.getById({ userId: this.userId })
        .pipe(
          map(x => this.user$.next(x)),
          switchMap(x => this.user$),
          map(x => this.form.patchValue({
            name: x.name
          }))
        )
        .subscribe();
  }

  public onDestroy: Subject<void> = new Subject<void>();

  ngOnDestroy() {
    this.onDestroy.next();	
  }

  public user$: BehaviorSubject<User> = new BehaviorSubject(<User>{});
  
  public userId: string;

  public handleCancelClick() {
    this._overlay.close();
  }

  public handleSaveClick() {
    const user = new User();
    user.userId = this.userId;
    user.name = this.form.value.name;
    this._userService.create({ user })
      .pipe(
        map(x => user.userId = x.userId),
        tap(x => this._overlay.close(user)),
        takeUntil(this.onDestroy)
      )
      .subscribe();
  }

  public form: FormGroup = new FormGroup({
    name: new FormControl(null, [])
  });
} 
