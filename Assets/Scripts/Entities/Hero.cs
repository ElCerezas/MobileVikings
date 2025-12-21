using System;
using UnityEngine;

public abstract class Hero : Unit, IMove, IAttack
{
    [Header("Base Components")]
    [SerializeField] protected int movementRange = 1;
    [SerializeField] protected MovementPattern mPattern;
    [SerializeField] protected int attackRange = 1;
    [SerializeField] protected AttackPattern aPattern;
    
    public virtual void Move(Action onFinished)
    {
        //CodiPerMoure
        //Debug.Log($"Mogut a [{mPattern.Move(currentTile, 1, owner).x} , {mPattern.Move(currentTile, 1, owner).y}]");
    }
    public virtual void Attack(Action onFinished)
    {
       // Debug.Log($"Atacat a [{mPattern.Move(currentTile, 1, owner).x} , {mPattern.Move(currentTile, 1, owner).y}]");
    }
}
