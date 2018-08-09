import { Component, ViewChild, Input } from "@angular/core";
import { Subject } from "rxjs";
import { IgxGridComponent } from "igniteui-angular";

@Component({
  templateUrl: "./grid.component.html",
  styleUrls: ["./grid.component.css"],
  selector: "cs-grid"
})
export class GridComponent { 

  @Input()
  public columns: any[] = [];

  @ViewChild("grid")
  grid: IgxGridComponent;

  @Input()
  data: any;

  afterViewInit: boolean;

  ngAfterViewInit() {
    this.afterViewInit = true;
  }

  ngDoCheck() {
    if (this.grid && this.afterViewInit)
      this.grid.reflow();
  }
}
