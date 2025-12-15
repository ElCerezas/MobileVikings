using UnityEngine;

public class Tile : MonoBehaviour
{
    public Vector2Int gridPosition { get; private set; }
    public Unit unitInTile { get; private set; }

    public void Init(Vector2Int gridPos)
    {
        gridPosition = gridPos;
    }
    public bool IsEmpty()
    {
        return unitInTile == null;
    }
    public void SetUnitInTile(Unit unit)
    {
        unitInTile = unit;
        unit.currentTile = this;
    }
    public void ClearUnit()
    {
        if (unitInTile != null)
            unitInTile.currentTile = null;

        unitInTile = null;
    }
}