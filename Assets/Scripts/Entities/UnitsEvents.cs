using System;

public interface IStartTurn
{
    void StartTurn(Action onFinished);
}
public interface IBeforeMove
{
    void BeforeMove(Action onFinished);
}
public interface IMove
{
    void Move(Action onFinished);
}
public interface IAfterMove
{
    void AfterMove(Action onFinished);
}
public interface IBeforeAttack
{
    void BeforeAttack(Action onFinished);
}
public interface IAttack
{
    void Attack(Action onFinished);
}
public interface IAfterAttack
{
    void AfterAttack(Action onFinished);
}