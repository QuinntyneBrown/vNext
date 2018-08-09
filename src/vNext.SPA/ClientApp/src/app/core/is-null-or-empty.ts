import { deepCopy } from "./deep-copy";

export function isNullOrEmpty(value: string) {
  var _value = deepCopy(value);

  if (_value && _value.length > 0) _value = _value.trim();
  return !_value || _value == "";
}
