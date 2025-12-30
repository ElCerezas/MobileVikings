using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattleController : MonoBehaviour
{
    [Header("CoreSystems")]
    [SerializeField] GridSystem gridSystem;
    [SerializeField] LevelLoader levelLoader;
    [SerializeField] EnemyModule enemyModule;

    [Header("LevelGeneration")]
    [SerializeField] int levelId = 1;

    [Header("TurnManager")]
    bool playerTurn = true;
    public delegate void PlacementEnded();

    [SerializeField]List<Unit> ActivePlayerUnits;
    [SerializeField]List<Unit> ActiveEnemyUnits;

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
        MovePhase();
    }
    void MovePhase()
    {
        List<Unit> activeUnits = playerTurn ? ActivePlayerUnits : ActiveEnemyUnits;
        ExecutePhase<IBeforeMove>(activeUnits, (u, cb) => u.BeforeMove(cb), () => {
            ExecutePhase<IMove>( activeUnits, (u, cb) => u.Move(cb), () => {
                ExecutePhase<IAfterMove> (activeUnits, (u, cb) => u.AfterMove(cb), () =>
                    {
                        Debug.Log("MovePhase terminada");
                        AtackPhase();
                    }
                );
            });
        });
    }
    void AtackPhase()
    {
        List<Unit> activeUnits = playerTurn ? ActivePlayerUnits : ActiveEnemyUnits;
        ExecutePhase<IBeforeAttack>(activeUnits, (u, cb) => u.BeforeAttack(cb), () => {
            ExecutePhase<IAttack>(activeUnits, (u, cb) => u.Attack(cb), () => {
                ExecutePhase<IAfterAttack>(activeUnits, (u, cb) => u.AfterAttack(cb), () =>
                {
                    Debug.Log("AttackPhase terminada");
                    PlaceFase();
                }
                );
            });
        });
    }
    void PlaceFase()
    {
        if (playerTurn)
        {
            //PlayerManager.Instance.OnPlacementPhase(() => { OnPlacementPhaseEnded();} );
        }
        else
        {
            enemyModule.OnPlacementPhase(() => { OnPlacementPhaseEnded(); });
        }
    }
    void OnPlacementPhaseEnded()
    {
        Debug.Log("Placement terminado");
        playerTurn = !playerTurn;
        StartTurn();
    }

    //Aixo crec q està be
    void ExecutePhase<T>(List<Unit> units, Action<T, Action> accion, Action onPhaseFinished) where T : class
    {
        int pending = 0;

        for (int i = 0; i < units.Count; i++) 
        {
            if (units[i] is T phase)
            {
                pending++;
                accion(phase, () => { pending--; if (pending == 0) onPhaseFinished?.Invoke(); });
            }
        }
        if (pending == 0) onPhaseFinished?.Invoke();
    }
}
