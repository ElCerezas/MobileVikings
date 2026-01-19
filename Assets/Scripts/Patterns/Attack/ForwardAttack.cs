[System.Serializable]
public class ForwardAttack : AttackPattern
{
    public override Tile[] Attack(Tile actualTile, int attackRange, UnitOwner owner)
    {
        Tile[] targetTiles = new Tile[attackRange];

        int currentX = actualTile.x;
        int currentY = actualTile.y;

        int directionY = owner == UnitOwner.Player ? -1 : 1;

        for (int i = 0; i < attackRange; i++)
        {
            int targetY = currentY + (directionY * (i + 1));
            targetTiles[i] = (GridSystem.instance.GetTile(currentX, targetY));
        }

        return targetTiles[0] != null ? targetTiles : null;
    }
}