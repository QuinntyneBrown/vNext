import { Component } from "@angular/core";
import { Subject, Observable } from "rxjs";
import { ContactService } from "./contact.service";
import { Contact } from "./contact.model";
import { IgxColumnComponent } from "igniteui-angular";
import { Router } from "@angular/router";

@Component({
  templateUrl: "./contacts-page.component.html",
  styleUrls: ["./contacts-page.component.css"],
  selector: "app-contacts-page"
})
export class ContactsPageComponent { 
  constructor(
    private _contactService: ContactService,
    private _router: Router
  ) {

  }

  ngOnInit() {
    this.contacts$ = this._contactService.get();
  }

  public onDestroy: Subject<void> = new Subject<void>();

  public columns: { field: string, header: string, pinned?: string }[] = [
    {
      field: "contactId",
      header: "Id"
    },
    {
      field: "firstName",
      header: "First Name",
      pinned: "true"
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

  handleCreateClick() {
    this._router.navigateByUrl("/contact/create");
  }

  onCellClick($event) {
    this._router.navigateByUrl(`/contact/edit/${$event.cell.row.rowData.contactId}`);
  }

  public contacts$: Observable<Contact[]>;
}
