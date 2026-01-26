using System.Collections.Generic;
using UnityEngine;

public class GridSystem : MonoBehaviour
{
    public static GridSystem instance { get; private set; }
    private Tile[,] tiles;
    public int Width { get; private set; }
    public int Height { get; private set; }

    private void Awake()
    {
        instance = this;
    }
    public void Initialize(int w, int h)
    {
        Width = w;
        Height = h;
        tiles = new Tile[w, h];
    }
    public Tile GetTile(int x, int y)
    {
        if (x < 0 || y < 0 || x >= Width || y >= Height)
            return null;
        return tiles[x, y];
    }
    public void RegisterTile(Tile tile)
    {
        if (tile.x >= 0 && tile.x < Width && tile.y >= 0 && tile.y < Height)
        {
            tiles[tile.x, tile.y] = tile;
        }
    }
    public Tile[,] GetAllTiles()
    {
        return tiles;
    }
    public List<Tile> GetRegisteredTiles(TileOwner tileOwner)
    {
        List<Tile> placeableTiles = new List<Tile>();
        for (int x = 0; x < GridSystem.instance.Width; x++)
        {
            for (int y = 0; y < GridSystem.instance.Height; y++)
            {
                Tile currentTile = GetAllTiles()[x, y];
                if (currentTile != null && currentTile.owner == TileOwner.Enemy)
                {
                    placeableTiles.Add(currentTile);
                }
            }
        }
        return placeableTiles;
    }

}
