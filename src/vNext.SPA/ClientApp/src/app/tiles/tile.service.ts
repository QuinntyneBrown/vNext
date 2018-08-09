import { HttpClient } from "@angular/common/http";
import { Inject, Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { map } from "rxjs/operators";
import { Tile } from "./tile.model";
import { baseUrl } from "../core/constants";

@Injectable()
export class TileService {
  constructor(
    @Inject(baseUrl) private _baseUrl: string,
    private _client: HttpClient) {

  }

  public list(): Observable<Array<Tile>> {
    return this._client.get<{ tiles: Array<Tile> }>(`${this._baseUrl}api/tiles`)
      .pipe(
        map(x => x.tiles)
      );
  }

}
