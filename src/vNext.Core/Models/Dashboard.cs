using System.Collections.Generic;

namespace vNext.Core.Models
{
    public class Dashboard
    {
        public short DashboardId { get; set; } = 0;
        public string Code { get; set; }
        public string Settings { get; set; }
        public int Sort { get; set; }
        public int UserId { get; set; }
        public ICollection<DashboardTile> DashboardTiles { get; set; } = new HashSet<DashboardTile>();
        public int ConcurrencyVersion { get; set; }
    }

}
