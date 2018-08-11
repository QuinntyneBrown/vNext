import { Component } from "@angular/core";
import { Subject, Observable } from "rxjs";
import { UserService } from "./user.service";
import { User } from "./user.model";

@Component({
  templateUrl: "./users-page.component.html",
  styleUrls: ["./users-page.component.css"],
  selector: "app-users-page"
})
export class UsersPageComponent { 
  constructor(
    private _userService: UserService
  ) { }

  public columns: { field: string, header: string }[] = [
    {
      field: "userId",
      header: "Id"
    },
    {
      field: "code",
      header: "Code"
    }
  ];

  public ngOnInit() {
    this.users$ = this._userService.get();
  }

  public onDestroy: Subject<void> = new Subject<void>();

  ngOnDestroy() {
    this.onDestroy.next();	
  }

  public users$: Observable<User[]>;
}
