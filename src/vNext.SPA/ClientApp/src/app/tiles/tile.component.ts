import { Component, HostListener, HostBinding } from "@angular/core";
import { Subject } from "rxjs";

@Component({
  templateUrl: "./tile.component.html",
  styleUrls: ["./tile.component.css"],
  selector: "cs-tile"
})
export class TileComponent { 

  public onDestroy: Subject<void> = new Subject<void>();

  @HostListener('click', ['$event'])
  handleClick() {
    this.isSelected = !this.isSelected;
  }

  @HostBinding('class.selected')
  isSelected: boolean = false;

  ngOnDestroy() {
    this.onDestroy.next();	
  }
}
