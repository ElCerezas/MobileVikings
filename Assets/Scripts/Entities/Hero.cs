using SerializeReferenceEditor;
using System;
using System.Collections;
using UnityEngine;
public abstract class Hero : Unit, IMove, IAttack
{
    [Header("Stats")]
    [SerializeField] protected int damage;
    [SerializeField] protected bool piercingAttack = false;
    [SerializeField] protected bool affectsAllies = false;
    [SerializeField] protected bool affectsEnemies = true;

    [Header("Patterns")]
    [SerializeField][Min(1)] protected int movementRange = 1;
    [SerializeReference, SR] protected MovementPattern mPattern = null;
    [SerializeField] protected int attackRange = 1;
    [SerializeReference, SR] protected AttackPattern aPattern = null;

    [Header("Flags")]
    [SerializeField] protected bool enemyKilled;
    [SerializeField] protected bool moved;
    [SerializeField] protected bool attacked;


    public virtual void Move(Action onFinished)
    {
        Debug.Log(name + " Moved");
        moved = false;
        if (mPattern == null || currentTile == null)
        {
            onFinished?.Invoke();
            return;
        }
        Tile[] tiles = mPattern.Move(currentTile, movementRange, owner);
        if (tiles == null || tiles.Length == 0)
        {
            onFinished?.Invoke();
            return;
        }

        Tile destination = currentTile;

        foreach (Tile tile in tiles)
        {
            if (!tile.IsFree) break;
            destination = tile;
        }
        if (destination == currentTile)
        {
            onFinished?.Invoke();
            return;
        }

        currentTile.EmptyTile();
        destination.SetNewOccupant(this);
        currentTile = destination;
        moved = true;

        StartCoroutine(MoveCoroutine(destination.transform.position, onFinished));
    }
    public virtual void Attack(Action onFinished)
    {
        attacked = false;
        enemyKilled = false;
        if (aPattern == null || currentTile == null)
        {
            onFinished?.Invoke();
            return;
        }
        Tile[] tiles = aPattern.Attack(currentTile, attackRange, owner, piercingAttack, affectsEnemies, affectsAllies);

        if (tiles == null || tiles.Length == 0)
        {
            onFinished?.Invoke();
            return;
        }

        foreach (Tile tile in tiles)
        {
            if (tile.IsFree) continue;

            Unit target = tile.occupant;
            if (target == null) continue;

            bool isEnemy = target.GetOwner() != owner;

            if (isEnemy && !affectsEnemies) continue;
            if (!isEnemy && !affectsAllies) continue;

            // Apply damage
            target.ReceiveDamage(damage);
            attacked = true;
            if (target.IsDead())
            {
                enemyKilled = true;
            }

            if (!piercingAttack) break;
        }

        onFinished?.Invoke();
    }
    protected IEnumerator MoveCoroutine(Vector3 targetPosition, Action onFinished)
    {
        float speed = 5f;

        while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
        {
            transform.position = Vector3.MoveTowards( transform.position, targetPosition, speed * Time.deltaTime);
            yield return null;
        }

        transform.position = targetPosition;
    }
}
