import { AddressPhoneType } from "./address-phone-type.model";

export class AddressPhone {
  public addressPhoneId: number;
  public addressPhoneTypeId: number;
  public phone: string;
  public sort: number;
  public addressPhoneType: AddressPhoneType;
}
