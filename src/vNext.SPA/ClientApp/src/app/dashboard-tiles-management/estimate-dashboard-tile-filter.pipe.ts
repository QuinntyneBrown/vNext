import { Pipe } from "@angular/core";

@Pipe({
  name: 'estimateDashboarTileFilter',
  pure: false
})
export class EstimateDashboardTileFilterPipe {
  transform(items: any[]): any {
    if (!items) { return items; }

    return items.filter(item => item.tileId == 6);
  }
}
