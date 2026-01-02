using System;
using UnityEngine;

public class Warrior : Hero, IAfterAttack
{
    public void AfterAttack(Action onFinished)
    {
        if (enemyKilled)
        {
            damage += abilityModifiers[tier - 1];
        }
        enemyKilled = false;
        onFinished?.Invoke();
    }
}
