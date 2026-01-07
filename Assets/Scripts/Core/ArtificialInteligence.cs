using static BattleController;

using System.Linq;
using UnityEngine;

public class ArtificialInteligence : EnemyModule
{
    public override void OnPlacementPhase(PlacementEnded onFinished)
    {
        if (!battle.TryPeekUnitToPlace(forPlayer: false, out Unit unitPrefab))

        {
            onFinished?.Invoke();
            return;
        }

        Tile chosen = battle.Grid.AllTiles()
            .Where(t => battle.CanPlaceOn(t, isPlayer: false))
            .OrderBy(t => Random.value)
            .FirstOrDefault();

        if (chosen == null)
        {
            Debug.Log("IA: no hay tiles enemigas libres para colocar.");
            onFinished?.Invoke();
            return;
        }

        battle.PlaceUnitOn(chosen, unitPrefab, isPlayer: false);
        battle.ConsumeUnitToPlace(forPlayer: false, unitPrefab);
        onFinished?.Invoke();
    }
}
