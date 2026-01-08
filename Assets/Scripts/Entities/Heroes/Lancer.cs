using System;
using UnityEngine;

public class Lancer : Hero, IBeforeAttack
{
    public void BeforeAttack(Action onFinished)
    {
        throw new NotImplementedException();
    }
}
