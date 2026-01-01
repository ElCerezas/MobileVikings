using UnityEngine;
public enum UnitOwner {Player, Enemy}
public abstract class Unit : MonoBehaviour
{
    protected UnitOwner owner;
    protected Tile currentTile;

    [Header("Stats")]
    [SerializeField] [Range(1,3)] protected int tier = 1;
    [SerializeField] protected int maxLife;
    [SerializeField] protected int life;

    public virtual void Placement(Tile t)
    {
        //TODO
    }
}
