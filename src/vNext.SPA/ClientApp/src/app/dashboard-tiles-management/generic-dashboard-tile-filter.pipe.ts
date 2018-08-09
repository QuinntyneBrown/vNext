import { Pipe } from "@angular/core";

@Pipe({
  name: 'genericDashboarTileFilter',
  pure:false
})
export class GenericDashboardTileFilterPipe {
  transform(items: any[]): any {
    if (!items) { return items; }

    return items.filter(item => item.tileId != 6);
  }
}
