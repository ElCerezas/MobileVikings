using UnityEngine;
public enum UnitOwner {Player, Enemy}
public abstract class Unit : MonoBehaviour
{
    protected UnitOwner owner;
    protected Tile currentTile;

    [Header("Stats")]
    [SerializeField] protected int maxLife;
    [SerializeField] protected int life;
}
