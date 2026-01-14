
[System.Serializable]
public abstract class AttackPattern
{
    public abstract Tile[] Attack(Tile actualTile, int attackTiles, UnitOwner owner); //TargetedTile
}