[System.Serializable]
public class ForwardAttack : AttackPattern
{
    public override Tile[] Attack(Tile actualTile, int attackRange, UnitOwner owner, bool piercing, bool affectsEnemies, bool affectsAlies)
    {
        int currentX = actualTile.x;
        int currentY = actualTile.y;
        int directionY = owner == UnitOwner.Player ? 1 : -1;
        int targetY = currentY + (directionY * attackRange);

        Tile targetTile = GridSystem.Instance.GetTile(currentX, targetY);

        if (targetTile != null)
        {
            return new Tile[] { targetTile };
        }

        return new Tile[0];
    }
}