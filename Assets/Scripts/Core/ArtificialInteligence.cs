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

        List<Tile> tile = GetPlaceableEnemyTiles();

        if (tile == null || tile.Count == 0)
        {
            onFinished?.Invoke();
            return;
        }

        chooseBestMove(deck, tile, out bestUnit, out bestTile);
        UnitPlacementManager.instance.PlaceEnemyUnit(bestUnit, bestTile);
        //UnitPlacementManager.instance.PlaceEnemyUnit(selectedUnit, GridSystem.instance.GetTile(0,0)); Utilitzar per provar col·locació d'unitats
        onFinished?.Invoke();
    }

    private List<Tile> GetPlaceableEnemyTiles()
    {
        Tile[,] allTies = GridSystem.instance.GetAllTiles();

        List<Tile> placeableTiles = new List<Tile>();
        for (int x = 0; x < GridSystem.instance.Width; x++)
        {
            for (int y = 0; y < GridSystem.instance.Height; y++) 
            { 
                Tile currentTile = allTies[x, y];
                if (currentTile != null && currentTile.owner == TileOwner.Enemy)  
                {
                    placeableTiles.Add(currentTile);
                }
            }
        }
        return placeableTiles;
    }

    private void chooseBestMove(List<Unit> deck, List<Tile> possibleTiles, out Unit bestUnit, out Tile bestTile)
    {
        bestUnit = null;
        bestTile = null;
    }
}
