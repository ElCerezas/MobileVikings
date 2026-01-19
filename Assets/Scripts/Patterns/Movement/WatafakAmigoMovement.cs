using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class WatafakAmigoMovement : MovementPattern
{
    public override Tile[] Move(Tile actualTile, int moveTiles, UnitOwner owner)
    {
        List<Tile> targetTiles = new List<Tile>();
        int currentX = actualTile.x;
        int currentY = actualTile.y;
        int directionY = owner == UnitOwner.Player ? 1 : -1;

        //1( Avanza una vez en diagonal otra adelante cubrint 2 filas)
        Vector2Int[] knightOffsets = new Vector2Int[]
        {
            new Vector2Int(1, 2),   // Derecha 1, Adelante 2
            new Vector2Int(2, 1),   // Derecha 2, Adelante 1
            new Vector2Int(-1, 2),  // Izquierda 1, Adelante 2
            new Vector2Int(-2, 1)   // Izquierda 2, Adelante 1
        };

        foreach (Vector2Int offset in knightOffsets)
        {
            int targetX = currentX + offset.x;
            int targetY = currentY + (offset.y * directionY);

            Tile tile = GridSystem.instance.GetTile(targetX, targetY);
            if (tile != null) targetTiles.Add(tile);
        }

        return targetTiles.ToArray();
    }
}

