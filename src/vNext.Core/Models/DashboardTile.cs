namespace vNext.Core.Models
{
    public class DashboardTile
    {
        public int DashboardId { get; set; }
        public int DashboardTileId { get; set; }
        public int TileId { get; set; }
        public string Settings { get; set; }
        public int ConcurrencyVersion { get; set; }
    }
}
