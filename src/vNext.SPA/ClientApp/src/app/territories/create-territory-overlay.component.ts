import { Component } from "@angular/core";
import { Subject, BehaviorSubject } from "rxjs";
import { FormGroup, FormControl } from "@angular/forms";
import { OverlayRefWrapper } from "../core/overlay-ref-wrapper";
import { TerritoryService } from "./territory.service";
import { Territory } from "./territory.model";
import { map, switchMap, tap, takeUntil } from "rxjs/operators";

@Component({
  templateUrl: "./create-territory-overlay.component.html",
  styleUrls: ["./create-territory-overlay.component.css"],
  selector: "app-create-territory-overlay",
  host: { 'class': 'mat-typography' }
})
export class CreateTerritoryOverlayComponent { 
  constructor(
    private _territoryService: TerritoryService,
    private _overlay: OverlayRefWrapper) { }

  ngOnInit() {
    if (this.territoryId)
      this._territoryService.getById({ territoryId: this.territoryId })
        .pipe(
          map(x => this.territory$.next(x)),
          switchMap(x => this.territory$),
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

  public territory$: BehaviorSubject<Territory> = new BehaviorSubject(<Territory>{});
  
  public territoryId: string;

  public handleCancelClick() {
    this._overlay.close();
  }

  public handleSaveClick() {
    const territory = new Territory();
    territory.territoryId = this.territoryId;
    territory.name = this.form.value.name;
    this._territoryService.create({ territory })
      .pipe(
        map(x => territory.territoryId = x.territoryId),
        tap(x => this._overlay.close(territory)),
        takeUntil(this.onDestroy)
      )
      .subscribe();
  }

  public form: FormGroup = new FormGroup({
    name: new FormControl(null, [])
  });
} 
