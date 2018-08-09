import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'countryFilter',
  pure: true
})
export class CountryFilterPipe implements PipeTransform {
  transform(items: any[], countryId: number): any {
    if (!items || !countryId) {
      return items;
    }

    return items.filter(item => item.countryId == countryId);
  }
}
