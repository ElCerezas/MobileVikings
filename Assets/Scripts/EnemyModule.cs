public abstract class EnemyModule
{
    protected BattleController battle;

    public void Initialize(BattleController controller)
    {
        battle = controller;
    }

    public abstract void OnPlacementPhase();
}
