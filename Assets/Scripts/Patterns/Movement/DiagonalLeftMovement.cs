using System.Collections.Generic;

[System.Serializable]
public class DiagonalLeftMovement : MovementPattern
{
    public override Tile[] Move(Tile actualTile, int moveTiles, UnitOwner owner)
    {
        List<Tile> targetTiles = new List<Tile>();
        int currentX = actualTile.x;
        int currentY = actualTile.y;
        int directionY = owner == UnitOwner.Player ? 1 : -1;

        for (int i = 1; i <= moveTiles; i++)
        {
            int targetX = currentX - i;
            int targetY = currentY + (directionY * i);

        }

        return targetTiles.ToArray();
    }
}

