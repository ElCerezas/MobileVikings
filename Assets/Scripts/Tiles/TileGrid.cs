using System.Collections.Generic;
using UnityEngine;

public class TileGrid : MonoBehaviour
{
    [Header("Lists")]
    public List<Entity> enemies;
    public List<Entity> heroes;
    Tile[,] tiles;
    Vector2Int dimensions;
    public void Initialize(int w, int h)
    {
        tiles = new Tile[w, h];
    }
    public void SetTile(int x, int y, Tile t)
    {
        tiles[x, y] = t;
        t.y = y;
        t.x = x;
    }
    public Tile GetTile(int x, int y)
    {
        return tiles[x, y];
    }
    public void SetGridPosition(LevelData level)
    {
        transform.position = new Vector3(-(level.dimensions[1] / 2f), 0, (level.dimensions[1]));
    }
}

