using vNext.API.Features.Tiles;
using vNext.Core.Models;
using Newtonsoft.Json;

namespace vNext.API.Features.DashboardTiles
{
    public class DashboardTileDto
    {        
        public int DashboardTileId { get; set; }
        public int TileId { get; set; }
        public int DashboardId { get; set; }
        public dynamic Settings { get; set; }
        public TileDto Tile { get; set; }
        public int ConcurrencyVersion { get; set; }
        public static DashboardTileDto FromDashboardTile(dynamic dashboardTile)
        {
            var model = new DashboardTileDto
            {
                DashboardTileId = dashboardTile.DashboardTileId,
                DashboardId = dashboardTile.DashboardId,
                TileId = dashboardTile.TileId
            };

            switch (dashboardTile.TileId)
            {
                case 6:
                    model.Settings = JsonConvert.DeserializeObject<EstimateDashboardTileSettingsDto>(dashboardTile.Settings);
                    break;
                default:
                    model.Settings = JsonConvert.DeserializeObject<DashboardTileSettingsDto>(dashboardTile.Settings);
                    break;
            }
            
            model.ConcurrencyVersion = dashboardTile.ConcurrencyVersion;

            return model;
        }
    }
}
