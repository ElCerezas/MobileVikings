using static BattleController;
using System.Linq;
using UnityEngine;

public class ArtificialInteligence : EnemyModule
{
    public Unit selectedUnit;
    public override void OnPlacementPhase(PlacementEnded onFinished)
    {
        /*if (!battle.TryPeekUnitToPlace(forPlayer: false, out Unit unitPrefab))
        {
            onFinished?.Invoke();
            return;
        }*/

        /*Tile chosen = battle.Grid.GetAllTiles()[0, 0];
            /*Where(t => battle.CanPlaceOn(t, isPlayer: false))
            .OrderBy(t => Random.value)
            .FirstOrDefault();*/

        /*if (chosen == null)
        {
            Debug.Log("IA: no hay tiles enemigas libres para colocar.");
            onFinished?.Invoke();
            return;
        }*/

        UnitPlacementManager.instance.PlaceEnemyUnit(selectedUnit, GridSystem.instance.GetTile(0,0));
        onFinished?.Invoke();
    }
}
