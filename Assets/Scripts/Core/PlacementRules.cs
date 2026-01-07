using UnityEngine;

public static class PlacementRules
{
    public static bool CanPlace(UnitOwner unitOwner, Tile tile)
    {
        if (tile == null) return false;
        if (!tile.IsFree) return false;

        if (unitOwner == UnitOwner.Player && tile.owner != TileOwner.Player)
            return false;

        if (unitOwner == UnitOwner.Enemy && tile.owner != TileOwner.Enemy)
            return false;

        return true;
    }
}