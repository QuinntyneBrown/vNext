import { AddressEmailType } from "./address-email-type.model";

export class AddressEmail {
  public addressEmailId: number;
  public email: string;
  public addressEmailTypeId: number;
  public sort: number;
  public addressEmailType: AddressEmailType;
}
