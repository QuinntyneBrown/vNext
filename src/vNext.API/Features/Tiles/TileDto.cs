namespace vNext.API.Features.Tiles
{
    public class TileDto
    {        
        public int TileId { get; set; }
        public string Code { get; set; }

        public static TileDto FromTile(dynamic tile)
            => new TileDto
            {
                TileId = tile.TileId,
                Code = tile.Code
            };
    }
}
