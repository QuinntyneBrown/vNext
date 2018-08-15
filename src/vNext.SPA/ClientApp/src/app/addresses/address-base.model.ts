import { Country } from "./country.model";
import { CountrySubdivision } from "./country-subdivision.model";

export class AddressBase {
  public addressBaseId: number;
  public address: string = "";
  public city: string = "";
  public postalZipCode: string = "";
  public county: string = "";
  public country: Country;
  public countryId: number;
  public countrySubdivisionId: number;
  public countrySubdivision: CountrySubdivision;
}
