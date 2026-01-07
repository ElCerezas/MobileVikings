using UnityEngine;
using static BattleController;

public abstract class EnemyModule : MonoBehaviour
{
    protected BattleController battle;

    public void Initialize(BattleController controller)
    {
        battle = controller;
    }

    public abstract void OnPlacementPhase(PlacementEnded onFinished);
}