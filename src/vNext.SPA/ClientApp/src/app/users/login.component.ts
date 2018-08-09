import { Component, Input, Renderer, ElementRef, HostListener, Output, EventEmitter } from '@angular/core';
import { Subject } from 'rxjs';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { takeUntil, tap, map } from 'rxjs/operators';
import { MatSnackBarRef, SimpleSnackBar } from '@angular/material';
import { RedirectService } from '../core/redirect.service';
import { HttpErrorResponse } from '@angular/common/http';
import { NotificationService } from '../core/notification.service';

@Component({
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
  selector: 'app-login'
})
export class LoginComponent {
  constructor(
    private _elementRef: ElementRef,
    private _errorService: NotificationService,
    private _loginRedirectService: RedirectService,
    private _renderer: Renderer
  ) {}

  public onDestroy: Subject<void> = new Subject<void>();

  ngAfterContentInit() {
    this._renderer.invokeElementMethod(this.usernameNativeElement, 'focus', []);

    this.form.patchValue({
      username: this.username,
      password: this.password
    });
  }

  @Input()
  public username: string;

  @Input()
  public password: string;

  private _disabled: boolean = false;

  public get disabled() { return this._disabled; }

  @Input("disabled")
  public set disabled(value: boolean) {
    this._disabled = value;
    var _ = value ? this.form.disable() : this.form.enable();
  }

  private _snackBarRef: MatSnackBarRef<SimpleSnackBar>;

  public form = new FormGroup({
    username: new FormControl(this.username, [Validators.required]),
    password: new FormControl(this.password, [Validators.required])
  });

  public get usernameNativeElement(): HTMLElement {
    return this._elementRef.nativeElement.querySelector('#username');
  }

  @HostListener('window:click')
  public dismissSnackBar() {
    if (this._snackBarRef) this._snackBarRef.dismiss();
  }

  @Output()
  public tryToLogin: EventEmitter<any> = new EventEmitter();

  public handleErrorResponse(errorResponse) {
    this._snackBarRef = this._errorService.handleError(errorResponse, 'Login Failed');
  }

  ngOnDestroy() {
    this.onDestroy.next();
  }
}
