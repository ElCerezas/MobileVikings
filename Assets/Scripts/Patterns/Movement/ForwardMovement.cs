using System.Collections.Generic;

[System.Serializable]
public class ForwardMovement : MovementPattern
{
    public override Tile[] Move(Tile actualTile, int moveTiles, UnitOwner owner)
    {
        Tile[] targetTiles = new Tile[moveTiles];

        int currentX = actualTile.x;
        int currentY = actualTile.y;

        int directionY = owner == UnitOwner.Player ? -1 : 1;

        for (int i = 0; i < moveTiles; i++)
        {
            int targetY = currentY + (directionY * (i+1));
            targetTiles[i] = (BattleController.instance.Grid.GetTile(currentX, targetY));
        }

        return targetTiles;
    }
}

