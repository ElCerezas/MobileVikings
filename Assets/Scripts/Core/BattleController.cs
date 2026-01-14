using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattleController : MonoBehaviour
{
    [SerializeField]bool debugStop = false;

    [Header("CoreSystems")]
    [SerializeField] GridSystem gridSystem;
    public static BattleController instance;
    public GridSystem Grid => gridSystem;
    [SerializeField] LevelLoader levelLoader;
    public EnemyModule enemyModule;
    public PlayerPlacementModule playerModule;
    public int turnsToFinish = 5;
    [SerializeField] private int currentTurn = 0;


    [Header("LevelGeneration")]
    [SerializeField] int levelId = 1;

    [Header("TurnManager")]
    public bool playerTurn = true;
    public delegate void PlacementEnded();
    bool playerActedThisRound;
    bool enemyActedThisRound;


    [Header("Placement")]
    [SerializeField] List<Unit> playerUnitsToPlace; 
    [SerializeField] List<Unit> enemyUnitsToPlace;
    [SerializeField] List<Unit> ActivePlayerUnits = new();
    [SerializeField] List<Unit> ActiveEnemyUnits = new();

    public static int eventedUnits = 0;
    //public static event Action<bool> OnPlacementStatue;
    //public static event Action<bool> OnTurnStarted;
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        Debug.LogWarning("0.Start");
        levelLoader.GenerateLevel(levelLoader.LoadLevelFromResources(levelId));
        Debug.LogWarning("Level Loaded " + levelLoader);
        playerTurn = CoinFlip();
        PlaceStatues();
        playerModule.Initialize(this);
        enemyModule.Initialize(this);
        playerActedThisRound = false;
        enemyActedThisRound = false;

        StartTurn();
    }
    bool CoinFlip()
    {
        return UnityEngine.Random.Range(0, 2) == 0 ? false : true;
    }
    void PlaceStatues()
    {
        //Los dos jugadores placean su estatua, (segun playerTurn empieza uno o el otro) una vez acaba uno, lo hace el otro. A partir de ahi, se va directamente a la fase de Poner piezas  del jugador que empieza.
    }
    void StartTurn()
    {
        Debug.LogWarning("TURN STARTED "  + playerTurn);
        
        List<Unit> activeUnits = playerTurn ? ActivePlayerUnits : ActiveEnemyUnits;
        ExecutePhase<IStartTurn>(activeUnits, (u, cb) => u.StartTurn(cb), () => { MovePhase(); });
        
    }
    void MovePhase()
    {
        Debug.LogWarning("1.Move phase " + playerTurn);
        List<Unit> activeUnits = playerTurn ? ActivePlayerUnits : ActiveEnemyUnits;
        ExecutePhase<IBeforeMove>(activeUnits, (u, cb) => u.BeforeMove(cb), () => {
            ExecutePhase<IMove>( activeUnits, (u, cb) => u.Move(cb), () => {
                ExecutePhase<IAfterMove> (activeUnits, (u, cb) => u.AfterMove(cb), () =>
                    {
                        Debug.Log("MovePhase terminada " + playerTurn);
                        AtackPhase();
                    }
                );
            });
        });
    }
    void AtackPhase()
    {
        Debug.LogWarning("2.Attack phase " + playerTurn);
        List<Unit> activeUnits = playerTurn ? ActivePlayerUnits : ActiveEnemyUnits;
        ExecutePhase<IBeforeAttack>(activeUnits, (u, cb) => u.BeforeAttack(cb), () => {
            ExecutePhase<IAttack>(activeUnits, (u, cb) => u.Attack(cb), () => {
                ExecutePhase<IAfterAttack>(activeUnits, (u, cb) => u.AfterAttack(cb), () =>
                {
                    Debug.Log("AttackPhase terminada " + playerTurn);
                    PlaceFase();
                }
                );
            });
        });
    }
    void PlaceFase()
    {
        Debug.LogWarning("3.Place phase " + playerTurn);

        bool canPlayer = playerUnitsToPlace != null && playerUnitsToPlace.Count > 0;
        bool canEnemy = enemyUnitsToPlace != null && enemyUnitsToPlace.Count > 0;

        if (!canPlayer && !canEnemy)
        {
             EndTurn();
            return;
        }

        if (playerTurn && !canPlayer) { EndTurn(); return; }
        if (!playerTurn && !canEnemy) { EndTurn(); return; }

        if (playerTurn) playerModule.OnPlacementPhase(EndTurn);
        else enemyModule.OnPlacementPhase(EndTurn);
    }


    void EndTurn()
    {
        Debug.Log("Placement terminado / Fin de turno " + playerTurn);
        if (playerTurn) playerActedThisRound = true;
        else enemyActedThisRound = true;
        playerTurn = !playerTurn;

        bool anyToPlace = (playerUnitsToPlace != null && playerUnitsToPlace.Count > 0) || (enemyUnitsToPlace != null && enemyUnitsToPlace.Count > 0);
        bool anyActive = (ActivePlayerUnits != null && ActivePlayerUnits.Count > 0) || (ActiveEnemyUnits != null && ActiveEnemyUnits.Count > 0);
        if(playerActedThisRound && enemyActedThisRound) currentTurn++;
        if (currentTurn >= turnsToFinish)
        {
            Debug.LogWarning("No hay unidades activas ni por colocar. Paro el loop de turnos.");
            return;
        }

        StartCoroutine(StartTurnNextFrame());
    }


    public bool CanPlaceOn(Tile tile, bool isPlayer) //S'ha de fer fix de terreny conquistat
    {
        if (tile == null) return false;
        if (!tile.IsFree) return false;
        if (isPlayer && tile.owner != TileOwner.Player) return false;
        if (!isPlayer && tile.owner != TileOwner.Enemy) return false;
        return true;
    }
    public Unit PlaceUnitOn(Tile tile, Unit unitPrefab, bool isPlayer) //TO DO: Revisar si val la pena usar dictionary per no tenir que instanciar.
    {
        Unit u = Instantiate(unitPrefab);
        Vector3 spawnPos = (tile.spawnPoint != null) ? tile.spawnPoint.position : tile.transform.position;
        u.transform.position = spawnPos;
        tile.SetNewOccupant(u);

        u.Placement(tile, isPlayer ? UnitOwner.Player : UnitOwner.Enemy, 1); //TO DO calcular el tier segons la distancia

        RegisterPlacedUnit(u, isPlayer);
        return u;
    }
    public void RegisterPlacedUnit(Unit unit, bool isPlayer) //Afegir unitat a unitats activas
    {
        if (isPlayer) ActivePlayerUnits.Add(unit);
        else ActiveEnemyUnits.Add(unit);
    }
 
    System.Collections.IEnumerator StartTurnNextFrame()
    {
        if (debugStop)
        {
            while (!Input.GetKeyDown(KeyCode.N))
            {
                yield return null;
            }

            StartTurn();
        }
        else
        {
            yield return null;
            StartTurn();
        }
            
    }

    /*public bool TryPeekUnitToPlace(bool forPlayer, out Unit unitPrefab) //NO ENTENC - ADRI Aixo ho vaig fer perque hi havia un error que hem borrava les unitats a colocar abans de temps, potser ara ja no es necessari pero serveix per seguratat 
    {
        var list = forPlayer ? playerUnitsToPlace : enemyUnitsToPlace;
        if (list == null || list.Count == 0)
        {
            unitPrefab = null;
            return false;
        }
        unitPrefab = list[0];   
        return true;
    }*/

    public bool ConsumeUnitToPlace(bool forPlayer, Unit expectedPrefab) //Eliminar unitat placed de unitats placed
    {
        var list = forPlayer ? playerUnitsToPlace : enemyUnitsToPlace;
        if (list == null || list.Count == 0) return false;

        if (list[0] != expectedPrefab) return false;

        list.RemoveAt(0);
        return true;
    }

    void ExecutePhase<T>(List<Unit> units, Action<T, Action> accion, Action onPhaseFinished) where T : class //Executar fase :)
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