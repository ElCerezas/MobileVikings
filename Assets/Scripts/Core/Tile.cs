using UnityEngine;
public enum TileOwner {Player, Enemy, Neutral}
public class Tile : MonoBehaviour
{
    public int x;
    public int y;

    public Unit occupant;
    public TileOwner owner;// { get; private set; }

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
    public UnitOwner GetOccupantSide()
    {
        return occupant.GetOwner();
    }
    public void SetOwner(TileOwner newOwner)
    {
        owner = newOwner;
    }
    public void OnMouseDown()
    {
        if (!BattleController.instance.playerTurn) return;
        BattleController.instance.playerModule.TryPlaceUnit(this);
    }

}
