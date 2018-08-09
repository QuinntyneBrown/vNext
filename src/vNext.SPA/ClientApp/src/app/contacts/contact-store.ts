import { BehaviorSubject } from "rxjs";
import { Contact } from "./contact.model";

export class ContactStore {
  constructor() {
    this.currentContact$ = new BehaviorSubject(new Contact())
  }

  public currentContact$: BehaviorSubject<Contact>;
}
