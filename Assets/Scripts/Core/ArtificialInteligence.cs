using static BattleController;

public class ArtificialInteligence : EnemyModule
{
    public override void OnPlacementPhase(PlacementEnded onFinished)
    {
        onFinished?.Invoke();
    }
}