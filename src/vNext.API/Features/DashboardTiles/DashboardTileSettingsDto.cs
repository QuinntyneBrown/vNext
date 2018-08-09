namespace vNext.API.Features.DashboardTiles
{
    public class DashboardTileSettingsDto
    {
        public int Top { get; set; } = 1;
        public int Left { get; set; } = 1;
        public int Height { get; set; } = 1;
        public int Width { get; set; } = 1;
    }

    public class EstimateDashboardTileSettingsDto : DashboardTileSettingsDto
    {
        public int Count { get; set; } = 50;
    }
}
