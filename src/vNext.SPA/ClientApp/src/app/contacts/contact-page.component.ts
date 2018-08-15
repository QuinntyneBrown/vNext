import { Component, ViewChild, HostBinding, ViewContainerRef, ComponentRef, ComponentFactoryResolver, Injector } from "@angular/core";
import { FormControl, FormGroup, Validators } from "@angular/forms";
import { MatDialog } from "@angular/material";
import { Router, ActivatedRoute } from "@angular/router";
import { Subject, BehaviorSubject } from "rxjs";
import { map, takeUntil, tap, switchMap } from "rxjs/operators";
import { AddressConverter } from "../addresses/address-converter";
import { AddressService } from "../addresses/address.service";
import { EditAddressOverlay } from "../addresses/edit-address-overlay";
import { IDeactivatable } from "../core/deactivable";
import { DiscardChangesConfirmationOverlayComponent } from "../shared/discard-changes-confirmation-overlay.component";
import { ContactStore } from "./contact-store";
import { Contact } from "./contact.model";
import { ContactService } from "./contact.service";
import { deepCopy } from "../core/deep-copy";
import { Address } from "../addresses/address.model";
import { ContactEditorComponent } from "./contact-editor.component";
import { EditAddressOverlayComponent } from "../addresses/edit-address-overlay.component";
import { EditAddressSideBarComponent } from "../addresses/edit-address-side-bar.component";
import { LocalStorageService } from "../core/local-storage.service";

@Component({
  templateUrl: "./contact-page.component.html",
  styleUrls: ["./contact-page.component.css"],
  selector: "cs-contact-page"
})
export class ContactPageComponent implements IDeactivatable { 
  public canDeactivate() {
    if (this.contactEditor.form.dirty) {
      let dialogRef = this._dialog.open(DiscardChangesConfirmationOverlayComponent, {
        width: '250px'
      });
      return dialogRef.afterClosed();    
    }
    return true;
  }

  constructor(
    private _activatedRoute: ActivatedRoute,
    private _addressConvert: AddressConverter,
    private _addressService: AddressService,
    private _componentFactoryResolver: ComponentFactoryResolver,
    private _contactService: ContactService,
    private _contactStore: ContactStore,
    private _dialog: MatDialog,
    private _router: Router,
    private _storage: LocalStorageService,
    private _editAddressOverlay: EditAddressOverlay,
    private _injector: Injector

  ) {
    this.handleAddressOverlayResult = this.handleAddressOverlayResult.bind(this);    
  }

  @ViewChild("contactEditor")
  public contactEditor: ContactEditorComponent;

  @ViewChild("target", { read: ViewContainerRef })
  target: ViewContainerRef;

  public onDestroy: Subject<void> = new Subject<void>();

  public ngOnInit() {
    const contactId = +this._activatedRoute.snapshot.params["id"];

    if (contactId) {
      this._contactService.getById({ contactId })
        .pipe(
          switchMap((contact: Contact) => {
            this.contact$.next(contact);
          
            return this._addressService.getById({ addressId: contact.addressId });
          }),
          map(address => {
            this.address$.next(address);
          }),
          takeUntil(this.onDestroy))
        .subscribe();
    }
  }

  public handleSave($event:Contact) {
    const contact = new Contact();
    contact.contactId = $event.contactId;
    contact.addressId = $event.addressId;
    contact.companyName = $event.companyName;
    contact.firstName = $event.firstName;
    contact.middleName = $event.middleName;
    contact.lastName = $event.lastName;
    
    this._contactService.save({ contact: contact })
      .pipe(
        map(x => {
          contact.contactId = x.contactId;
          this.contact$.next(contact);
        }),
        takeUntil(this.onDestroy)
      )
      .subscribe();
  }
  
  public contact$: BehaviorSubject<Contact> = new BehaviorSubject(<Contact>{});

  public address$: BehaviorSubject<Address> = new BehaviorSubject(new Address());

  public handleEditAddressClick($event) {
    this.sideBarOpen = true;

    let componentFactory = (<any>this._componentFactoryResolver.resolveComponentFactory(EditAddressSideBarComponent));

    const ref: ComponentRef<EditAddressSideBarComponent> = this.target.createComponent(<any>componentFactory, null, this._injector);

    ref.instance.cancel
      .pipe(
      tap(x => {
        this.sideBarOpen = false;
        ref.destroy();
      })
    ).subscribe();

    ref.instance.save
      .pipe(
        tap(x => {
          this.sideBarOpen = false;
          ref.destroy();
          this.handleAddressOverlayResult(x.addressId);
        })
      ).subscribe();
  }

  public handleAddressOverlayResult(addressId: any) {
    this.sideBarOpen = false;
    
    if (!addressId) return;

    this._addressService
      .getById({ addressId: addressId})
      .pipe(
        takeUntil(this.onDestroy),
        map(x => {
          this.address$.next(x);
        })
      )
      .subscribe();
  }
  
  ngOnDestroy() {
    this.onDestroy.next();	
  }

  @HostBinding("class.side-bar-open")
  public sideBarOpen: boolean = false;
}
