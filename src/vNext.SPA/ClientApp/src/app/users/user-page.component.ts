import { Component } from "@angular/core";
import { Subject, Observable } from "rxjs";
import { UserService } from "./user.service";

@Component({
  templateUrl: "./user-page.component.html",
  styleUrls: ["./user-page.component.css"],
  selector: "app-user-page"
})
export class UserPageComponent { 
  constructor(private _userService: UserService) {

  }

  public onDestroy: Subject<void> = new Subject<void>();

  ngOnDestroy() {
    this.onDestroy.next();	
  }
}
