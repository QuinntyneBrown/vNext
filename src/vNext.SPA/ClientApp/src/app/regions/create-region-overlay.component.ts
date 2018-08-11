import { Component } from "@angular/core";
import { Subject, BehaviorSubject } from "rxjs";
import { FormGroup, FormControl } from "@angular/forms";
import { OverlayRefWrapper } from "../core/overlay-ref-wrapper";
import { RegionService } from "./region.service";
import { Region } from "./region.model";
import { map, switchMap, tap, takeUntil } from "rxjs/operators";

@Component({
  templateUrl: "./create-region-overlay.component.html",
  styleUrls: ["./create-region-overlay.component.css"],
  selector: "app-create-region-overlay",
  host: { 'class': 'mat-typography' }
})
export class CreateRegionOverlayComponent { 
  constructor(
    private _regionService: RegionService,
    private _overlay: OverlayRefWrapper) { }

  ngOnInit() {
    if (this.regionId)
      this._regionService.getById({ regionId: this.regionId })
        .pipe(
          map(x => this.region$.next(x)),
          switchMap(x => this.region$),
          map(x => this.form.patchValue({
            name: x.name
          }))
        )
        .subscribe();
  }

  public onDestroy: Subject<void> = new Subject<void>();

  ngOnDestroy() {
    this.onDestroy.next();	
  }

  public region$: BehaviorSubject<Region> = new BehaviorSubject(<Region>{});
  
  public regionId: string;

  public handleCancelClick() {
    this._overlay.close();
  }

  public handleSaveClick() {
    const region = new Region();
    region.regionId = this.regionId;
    region.name = this.form.value.name;
    this._regionService.create({ region })
      .pipe(
        map(x => region.regionId = x.regionId),
        tap(x => this._overlay.close(region)),
        takeUntil(this.onDestroy)
      )
      .subscribe();
  }

  public form: FormGroup = new FormGroup({
    name: new FormControl(null, [])
  });
} 
