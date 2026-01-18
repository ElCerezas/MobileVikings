using Unity.VisualScripting;
using UnityEngine;
public enum UnitOwner {Player, Enemy}
public abstract class Unit : MonoBehaviour
{
    protected UnitOwner owner;
    protected Tile currentTile;
    [SerializeField] public bool isHero { get; protected set; }

    [Header("Stats")]
    [SerializeField][Range(1, 3)] protected int tier = 1;
    [SerializeField] protected int[] abilityModifiers = new int[3];
    [SerializeField][Min(1)] protected int maxLife;
    [SerializeField][Min(0)] protected int life;
    public UnitOwner GetOwner()
    {
        return owner;
    }
    public virtual void Placement(Tile t, UnitOwner unitOwner, int _tier)
    {
        life = maxLife;
        owner = unitOwner;
        currentTile = t;
        tier = _tier;
        //TODO
    }
    public virtual void ReceiveDamage(int amount)
    {
        life -= amount;
        life = Mathf.Max(life, 0);
        if (IsDead()) Die();
    }
    public virtual void HealDamage(int amount)
    {
        life += amount;
        life = Mathf.Min(life, maxLife);
    }
    public virtual bool IsDead()
    {
        return life <= 0;
    }
    public virtual void Die() //Quan vida >= -1
    {
        currentTile.EmptyTile();
        currentTile = null;
        ReturnToDeck();
    }
    public virtual void FinalRowScore() //Quan arriba al final del tauler
    {
        currentTile.EmptyTile();
        currentTile = null;
        ReturnToDeck();
    }
    public virtual void ReturnToDeck()
    {
        if (owner == UnitOwner.Player) BattleController.instance.ActivePlayerUnits?.Remove(this);
        else BattleController.instance.ActiveEnemyUnits?.Remove(this);
    }
}
