
[System.Serializable]
public abstract class AttackPattern
{
    public abstract Tile[] Attack(Tile actualTile, int moveTiles, UnitOwner owner, bool piercing, bool affectsEnemies, bool affectsAlies); //TargetedTile
}