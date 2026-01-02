using System;
using UnityEngine;

public class Warrior : Hero, IAfterAttack
{
    [Header("Ability Mods")]
    [SerializeField] bool enemyKilled;
    [SerializeField] int[] abilityModifiers;

    public void AfterAttack(Action onFinished)
    {
        if (enemyKilled)
        {
            damage += abilityModifiers[tier - 1];
        }
        enemyKilled = false;
        onFinished?.Invoke();
    }

    public override void Attack(Action onFinished)
    {
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
            if (target.IsDead())
            {
                enemyKilled = true;
            }

            if (!piercingAttack) break;
        }

        onFinished?.Invoke();
    }
}