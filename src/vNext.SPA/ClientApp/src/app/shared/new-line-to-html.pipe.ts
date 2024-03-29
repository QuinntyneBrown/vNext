import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'newLineToHtml',
  pure: true
})
export class NewLineToHtmlPipe implements PipeTransform {
  transform(value: string): string {
    if (!value) return value;
    return value.replace(/(?:\r\n|\r|\n)/g, '<br>');
  }
}
