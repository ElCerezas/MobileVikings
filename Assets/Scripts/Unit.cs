using System;
using UnityEngine;

public enum UnitOwner {Player, Enemy}
public abstract class Unit : MonoBehaviour, IMove, IAttack
{
    public UnitOwner owner;
    //[SerializeField] AttackPattern aPattern;
    //[SerializeField] MovementPattern mPattern;
    public virtual void Move(Action onFinished)
    {
        onFinished();
    }
    public virtual void Attack(Action onFinished)
    {
        onFinished();
    }

}