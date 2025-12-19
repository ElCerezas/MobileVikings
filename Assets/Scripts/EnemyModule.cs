using static BattleController;

public abstract class EnemyModule
{
    protected BattleController battle;

    public void Initialize(BattleController controller)
    {
        battle = controller;
    }

    public abstract void OnPlacementPhase(PlacementEnded onFinished); //onFinished?.Invoke(); Enviar aixo al final del placement
}
