using SerializeReferenceEditor;
using System;
using UnityEngine;
public abstract class Hero : Unit, IMove, IAttack
{
    [Header("Stats")]
    [SerializeField] protected int damage;

    [Header("Patterns")]
    [SerializeField] protected int movementRange = 1;
    [SerializeReference, SR] protected MovementPattern mPattern = null;
    [SerializeField] protected int attackRange = 1;
    [SerializeReference, SR] protected AttackPattern aPattern = null;

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
