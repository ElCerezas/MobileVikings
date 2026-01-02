using UnityEngine;

public class Tile : MonoBehaviour
{
    public int x;
    public int y;

    public Unit occupant;
    //public GridOwner owner;

    public bool IsFree = true;

    public void SetTileCoords(int _x, int _y)
    {
        x = _x; y = _y;
    }
    public void SetNewOccupant(Unit u)
    {
        occupant = u;
        IsFree = false;
    }
    public void EmptyTile()
    {
        occupant = null;
        IsFree = true;
    }
}
