import { AddressBase } from "./address-base.model";
import { AddressEmail } from "./address-email.model";
import { AddressPhone } from "./address-phone.model";

export class Address extends AddressBase {
  constructor() {
    super();
  }
  public addressId: number;
  public phone: string = '';
  public fax: string = '';
  public email: string = '';
  public website: string = '';
  public addressEmails: AddressEmail[] = [];
  public addressPhones: AddressPhone[] = []
}
