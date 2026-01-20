using Unity.VisualScripting;
using UnityEngine;
public enum UnitOwner {Player, Enemy}
[RequireComponent(typeof(UnitController))]
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

    public UnitController controller { get => GetComponent<UnitController>(); }
    public UnitOwner GetOwner()
    {
        return owner;
    }
    public virtual void Instantiate(UnitOwner owner)
    {
        this.owner = owner;
    }
    public virtual void Placement(Tile t, int _tier)
    {
        controller.TurnCollider(false);
        life = maxLife;
        currentTile = t;
        tier = _tier;
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
        BattleController.instance.UnitScore(owner, true);
        UnitPlacementManager.instance.ReturnToDeck(this);
    }
}
