using Newtonsoft.Json;
using System.Collections.Generic;
using vNext.API.Features.DashboardTiles;

namespace vNext.API.Features.Dashboards
{
    public class DashboardDto
    {        
        public int DashboardId { get; set; }
        public string Code { get; set; }
        public int UserId { get; set; }
        public DashboardSettingsDto Settings { get; set; }
        public int Sort { get; set; }
        public int ConcurrencyVersion { get; set; }
        public ICollection<DashboardTileDto> DashboardTiles { get; set; } 
            = new HashSet<DashboardTileDto>();

        public static DashboardDto FromDashboard(dynamic dashboard)
        {
            var dto = new DashboardDto
            {
                DashboardId = dashboard.DashboardId,
                Code = dashboard.Code,
                UserId = dashboard.UserId,
                ConcurrencyVersion = dashboard.ConcurrencyVersion,
                Settings = JsonConvert.DeserializeObject<DashboardSettingsDto>(dashboard.Settings)
            };

            foreach(var projection in dashboard.DashboardTiles)
            {
                var dashboardTile = DashboardTileDto.FromDashboardTile(projection);
                dto.DashboardTiles.Add(dashboardTile);
            }
            
            return dto;
        }
    }
}
