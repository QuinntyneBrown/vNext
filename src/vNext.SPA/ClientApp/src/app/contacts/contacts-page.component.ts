import { Component } from "@angular/core";
import { Subject, Observable } from "rxjs";
import { ContactService } from "./contact.service";
import { Contact } from "./contact.model";

@Component({
  templateUrl: "./contacts-page.component.html",
  styleUrls: ["./contacts-page.component.css"],
  selector: "app-contacts-page"
})
export class ContactsPageComponent { 
  constructor(
    private _contactService: ContactService
  ) {

  }

  ngOnInit() {
    this.contacts$ = this._contactService.get();
  }

  public onDestroy: Subject<void> = new Subject<void>();

  public columns: { field: string, header: string }[] = [
    {
      field: "contactId",
      header: "Id"
    },
    {
      field: "firstName",
      header: "First Name"
    },
    {
      field: "middleName",
      header: "Middle Name"
    },
    {
      field: "lastName",
      header: "Last Name"
    },
    {
      field: "companyName",
      header: "Company"
    }
  ];

  ngOnDestroy() {
    this.onDestroy.next();	
  }

  public contacts$: Observable<Contact[]>;
}
