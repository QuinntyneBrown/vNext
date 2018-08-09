import { Address } from "../addresses/address.model";

export class Contact {
  public contactId: number;
  public firstName: string;
  public middleName: string;
  public lastName: string;
  public companyName: string;
  public createdByUserId: number;
  public addressId: number;
  public address: Address;
  public createdDateTime: any;
  public noteText: string = '';
  public concurrencyVersion: number = 0;
}
