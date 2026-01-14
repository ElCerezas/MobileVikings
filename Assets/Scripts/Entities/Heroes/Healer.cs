using System;

public class Healer : Hero, IBeforeAttack
{
    public void BeforeAttack(Action onFinished)
    {
        Tile[] tiles = aPattern.Attack(currentTile, 1, owner);

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
        if (tiles[0].occupant.GetOwner() == owner)
        {
            tiles[0].occupant.HealDamage(abilityModifiers[tier - 1]);
        }
        onFinished?.Invoke();
        return;
    }
}
