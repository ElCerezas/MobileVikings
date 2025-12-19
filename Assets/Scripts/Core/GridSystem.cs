public class GridSystem
{
    private Tile[,] tiles;
    public int Width { get; private set; }
    public int Height { get; private set; }

    public void Initialize(int w, int h)
    {
        Width = w;
        Height = h;
        tiles = new Tile[w, h];
        /*
        for (int x = 0; x < w; x++)
            for (int y = 0; y < h; y++)
                cells[x, y] = new Tile { x = x, y = y, owner = GridOwner.Neutral };*/
    }

    public Tile GetTile(int x, int y)
    {
        if (x < 0 || y < 0 || x >= Width || y >= Height)
            return null;
        return tiles[x, y];
    }

}
