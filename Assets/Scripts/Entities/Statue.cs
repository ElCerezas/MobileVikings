using System;

public abstract class Statue : Unit, IStartTurn
{
    public void StartTurn(Action onFinished)
    {
        throw new NotImplementedException();
    }
}