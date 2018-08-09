import { Tile } from "../tiles/tile.model";

export class DashboardTile<T> { 
        
    public dashboardTileId: number = 0;

    public name?: string;

    public dashboardId: any;

    public tileId: any;

    public tile?: Tile = <Tile>{};
    
    public code: string;

    public userId?: any;

    public changeState?: string;

    public version?: number;

    public sort: any = 0;

    public auditLogNoteId: number;
    
    public concurrencyVersion: number;
  
    public settings: T = <T>{};  
    
}
