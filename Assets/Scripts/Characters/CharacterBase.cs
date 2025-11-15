// CharacterBase.cs
using UnityEngine;
using System;

public abstract class CharacterBase : MonoBehaviour
{
    public enum State { Patrol, Charge, Attack, Ultimate }
    protected State currentState;
    protected State previousState;

    protected float moveSpeed;
    protected float chargeSpeed;
    protected float attackRange;
    protected int health;
    protected Vector3 targetPosition; 

    protected virtual void OnEnable()
    {
        LevelManager.OnGameUpdate += CustomUpdate;
    }
    protected virtual void OnDisable()
    {
        LevelManager.OnGameUpdate -= CustomUpdate;
    }

    protected void ChangeState(State newState)
    {
        previousState = currentState;
        currentState = newState;
    }
    public virtual void CustomUpdate(float deltaTime)
    {
        switch (currentState)
        {
            case State.Patrol: UpdatePatrol(deltaTime); break;
            case State.Charge: UpdateCharge(deltaTime); break;
            case State.Attack: UpdateAttack(deltaTime); break;
            case State.Ultimate: UpdateUltimate(deltaTime); break;
        }
    }

    protected virtual void UpdatePatrol(float deltaTime) {}
    protected virtual void UpdateCharge(float deltaTime) {}
    protected virtual void UpdateAttack(float deltaTime) {}
    protected virtual void UpdateUltimate(float deltaTime) {}
    protected void MoveTowardsTarget(float deltaTime)
    {
        transform.position = Vector3.MoveTowards(transform.position,targetPosition, moveSpeed * deltaTime);
    }
}
