using static BattleController;
using System.Linq;
using UnityEngine;
using NUnit.Framework;
using System.Collections.Generic;

public class ArtificialInteligence : EnemyModule
{
    public enum Difficulty // convertir aixo en una classe
    {
        Easy, 
        Medium,
        Hard 
    }
    [SerializeField] private Difficulty difficulty = Difficulty.Medium;
    private Unit bestUnit;
    private Tile bestTile;

    public override void OnPlacementPhase(PlacementEnded onFinished)
    {
        var deck = UnitPlacementManager.instance.GetEnemyDeck();

        if (deck != null || deck.Count == 0) 
        {
            onFinished?.Invoke();
            return;
        }

        List<Tile> tile = GridSystem.instance.GetRegisteredTiles(TileOwner.Enemy);

        if (tile == null || tile.Count == 0)
        {
            onFinished?.Invoke();
            return;
        }

        chooseBestMove(deck, tile, out bestUnit, out bestTile);
        UnitPlacementManager.instance.PlaceEnemyUnit(bestUnit, bestTile);
        //UnitPlacementManager.instance.PlaceEnemyUnit(bestUnit, GridSystem.instance.GetTile(0,0)); //Utilitzar per provar col·locació d'unitats
        onFinished?.Invoke();
    }

    private void chooseBestMove(List<Unit> deck, List<Tile> possibleTiles, out Unit bestUnit, out Tile bestTile)
    {

        bestUnit = null;
        bestTile = null;
    }
}
