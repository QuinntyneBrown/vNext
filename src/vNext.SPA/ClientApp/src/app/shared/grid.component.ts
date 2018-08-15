import { Component, ViewChild, Input, NgZone, EventEmitter, Output } from "@angular/core";
import { Subject } from "rxjs";
import { IgxGridComponent } from "igniteui-angular";
import { debounce } from "../core/debounce";

@Component({
  templateUrl: "./grid.component.html",
  styleUrls: ["./grid.component.css"],
  selector: "cs-grid"
})
export class GridComponent { 
  constructor(
    private readonly _zone: NgZone
  ) {

  }
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

  @Output()
  cellClick: EventEmitter<any> = new EventEmitter();
  
  ngDoCheck() {
    if (this.grid && this.afterViewInit) {
      this.grid.reflow();
    }
  }
}
