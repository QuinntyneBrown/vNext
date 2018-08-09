import { FormGroup, ValidatorFn, ValidationErrors } from "@angular/forms";
import { isNullOrEmpty } from "../core/is-null-or-empty";

export const hasAddressValidator: ValidatorFn = (control: FormGroup): ValidationErrors | null => {
  const addressId = control.get('addressId');
  
  return isNullOrEmpty(addressId.value)
    ? { hasAddressInvalid: true }
    : null;
};
