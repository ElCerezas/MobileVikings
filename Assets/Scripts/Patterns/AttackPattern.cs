
[System.Serializable]
public abstract class AttackPattern
{
    public abstract Tile[] Attack(Tile actualTile, int moveTiles, UnitOwner owner); //TargetedTile
}