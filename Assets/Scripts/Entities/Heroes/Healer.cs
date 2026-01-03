using System;
using UnityEngine;

public class Healer : Hero, IBeforeMove
{
    public void BeforeMove(Action onFinished)
    {
        Tile[] tiles = aPattern.Attack(currentTile, 1, owner, false, false, true);

        if (tiles == null || tiles[0].IsFree)
        {
            onFinished?.Invoke();
            return;
        }
        if (!tiles[0].occupant.isHero)
        {
            onFinished?.Invoke();
            return;
        }

        tiles[0].occupant.HealDamage(abilityModifiers[tier-1]);
        onFinished?.Invoke();
        return;
    }
}
