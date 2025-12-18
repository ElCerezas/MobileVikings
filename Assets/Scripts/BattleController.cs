using System;
using System.Collections.Generic;
using UnityEngine;

public class BattleController : MonoBehaviour
{
    [Header("CoreSystems")]
    [SerializeField] GridSystem gridSystem;
    [SerializeField] LevelLoader levelLoader;
    [SerializeField] EnemyModule enemyModule;

    [Header("LevelGeneration")]
    [SerializeField] int levelId = 1;

    bool playerTurn = true;
    public static int eventedUnits = 0;
    bool initialTurn = true;
    public static event Action<bool> OnPlacementStatue;
    public static event Action<bool> OnTurnStarted;

    private void Start()
    {
        levelLoader.GenerateLevel(levelLoader.LoadLevelFromResources(levelId));
        playerTurn = CoinFlip();

    }
    bool CoinFlip()
    {
        return UnityEngine.Random.Range(0,1) == 0 ? false : true;
    }
    void PlaceStatues()
    {
        //Los dos jugadores placean su estatua, (segun playerTurn empieza uno o el otro) una vez acaba uno, lo hace el otro. A partir de ahi, se va directamente a la fase de Poner piezas  del jugador que empieza.
    }
    void StartTurn()
    {
        //SOLO LO HACEN LAS UNIDADES DEL JUGADOR QUE TOQUE
        //Manda el evento a todas las unidades OnBeforeMove y cuando hayan acabado todas manda el OnMove y luego el after move (en todos se espera a que todas las unidades se meuvan)
    }
    void MovePhase()
    {
        //SOLO LO HACEN LAS UNIDADES DEL JUGADOR QUE TOQUE
        //Manda el evento a todas las unidades OnBeforeMove y cuando hayan acabado todas manda el OnMove y luego el after move (en todos se espera a que todas las unidades se meuvan)

    }
    void AtackPhase()
    {
        //SOLO LO HACEN LAS UNIDADES DEL JUGADOR QUE TOQUE
        //Lo mismo que move phase pero con Attack
    }
    void PlaceFase()
    {
        //Cambia de turno y empieza de nuevo
    }
}