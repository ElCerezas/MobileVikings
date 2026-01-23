using static BattleController;
using System.Linq;
using UnityEngine;

public class ArtificialInteligence : EnemyModule
{
    public enum Difficulty // convertir aixo en una classe
    {
        Easy, 
        Medium,
        Hard 
    }
    [SerializeField] private Difficulty difficulty = Difficulty.Medium;
    public Unit selectedUnit;
    public override void OnPlacementPhase(PlacementEnded onFinished)
    {


        //UnitPlacementManager.instance.PlaceEnemyUnit(selectedUnit, GridSystem.instance.GetTile(0,0)); Utilitzar per provar col·locació d'unitats
        onFinished?.Invoke();
    }
}
