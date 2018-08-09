import { FormGroup, ValidatorFn, ValidationErrors } from "@angular/forms";
import { isNullOrEmpty } from "../core/is-null-or-empty";

export const contactValidator: ValidatorFn = (control: FormGroup): ValidationErrors | null => {
  const firstName = control.get('firstName');
  const lastName = control.get('lastName');
  const companyName = control.get('companyName');

  return (isNullOrEmpty(firstName.value)
    && isNullOrEmpty(lastName.value)
    && isNullOrEmpty(companyName.value))
    ? { contactInvalid: true }
    : null;
};
