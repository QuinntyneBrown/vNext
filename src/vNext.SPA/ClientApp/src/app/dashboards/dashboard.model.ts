import { DashboardSettings } from "./dashboard-settings.model";
import { DashboardTile } from "../dashboard-tiles/dashboard-tile.model";
import { DashboardTileSettings } from "../dashboard-tiles/dashboard-tile-settings.model";

export class Dashboard {
  constructor(code: string) {
    this.code = code;
  }

  public dashboardId?: number = 0;
  public code: string;
  public userId?: any;
  public changeState?: string;
  public version?: number;
  public sort?: any;
  public auditLogNoteId: number;
  public settings: DashboardSettings = new DashboardSettings();  
  public dashboardTiles: Array<DashboardTile<DashboardTileSettings>> = [];
  public concurrencyVersion: number;
}
