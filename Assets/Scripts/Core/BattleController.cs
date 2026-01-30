using System;
using System.Collections.Generic;
using UnityEngine;

public class BattleController : MonoBehaviour
{
    public static BattleController instance;
    [Header("CoreSystems")]
    [SerializeField] GridSystem gridSystem;
    [SerializeField] UnitPlacementManager placementManager;
    [SerializeField] LevelLoader levelLoader;
    [SerializeField] EnemyModule enemyModule;
    
    
    
    [SerializeField] private int currentTurn = 0;


    [Header("LevelGeneration")]
    [SerializeField] int levelId = 1;

    [Header("TurnManager")]
    public bool playerTurn = true;
    public delegate void PlacementEnded();
    bool playerActedThisRound;
    bool enemyActedThisRound;


    [Header("Placement")]
    //[SerializeField] List<Unit> playerUnitsToPlace; 
    //[SerializeField] List<Unit> enemyUnitsToPlace;
    public List<Unit> ActivePlayerUnits = new();
    public List<Unit> ActiveEnemyUnits = new();
    [Header("Scoring")]
    public int playerConquest = 3;
    public int enemmyConquest = 3;
    [SerializeField]Transform enemyPlane;
    [SerializeField]Transform playerPlane;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        /*//Debug.LogWarning("0.Start");
        levelLoader.GenerateLevel(levelLoader.LoadLevelFromResources(levelId));
        Debug.LogWarning("Level Loaded " + levelLoader);
        playerTurn = CoinFlip();
        //PlaceStatues();
        enemyModule.Initialize(this);
        playerActedThisRound = false;
        enemyActedThisRound = false;

        StartTurn();*/
    }
    public void StartPvPGame()
    {
        levelLoader.GenerateLevel(levelLoader.LoadLevelFromResources(levelId));
        playerTurn = CoinFlip();
        StartTurn();
    }
    bool CoinFlip()
    {
        return UnityEngine.Random.Range(0, 2) != 0;
    }
    void PlaceStatues()
    {
        //Los dos jugadores placean su estatua, (segun playerTurn empieza uno o el otro) una vez acaba uno, lo hace el otro. A partir de ahi, se va directamente a la fase de Poner piezas  del jugador que empieza.
    }
    void StartTurn()
    {
        //Debug.LogWarning("TURN STARTED "  + playerTurn);
        List<Unit> activeUnits = playerTurn ? ActivePlayerUnits : ActiveEnemyUnits;
        ExecutePhase<IStartTurn>(activeUnits, (u, cb) => u.StartTurn(cb), () => { MovePhase(); });
        
    }
    #region Phases
    void MovePhase()
    {
        //Debug.LogWarning("1.Move phase " + playerTurn);
        List<Unit> activeUnits = playerTurn ? ActivePlayerUnits : ActiveEnemyUnits;
        ExecutePhase<IBeforeMove>(activeUnits, (u, cb) => u.BeforeMove(cb), () => {
            ExecutePhase<IMove>( activeUnits, (u, cb) => u.Move(cb), () => {
                ExecutePhase<IAfterMove> (activeUnits, (u, cb) => u.AfterMove(cb), () =>
                    {
                        AtackPhase();
                    }
                );
            });
        });
        UpdateConquestTiles();
        CheckForGameEnd();
    }
    void AtackPhase()
    {
        UpdateConquestTiles();
        //Debug.LogWarning("2.Attack phase " + playerTurn);
        List<Unit> activeUnits = playerTurn ? ActivePlayerUnits : ActiveEnemyUnits;
        ExecutePhase<IBeforeAttack>(activeUnits, (u, cb) => u.BeforeAttack(cb), () => {
            ExecutePhase<IAttack>(activeUnits, (u, cb) => u.Attack(cb), () => {
                ExecutePhase<IAfterAttack>(activeUnits, (u, cb) => u.AfterAttack(cb), () =>
                {
                    PlaceFase();
                }
                );
            });
        });
        UpdateConquestTiles();
        CheckForGameEnd();
    }
    void PlaceFase()
    {
        if (playerTurn) placementManager.OnPlayerPlacementPhase(EndTurn);
        else enemyModule.OnPlacementPhase(EndTurn);
    }
    void EndTurn()
    {
        if (playerTurn) playerActedThisRound = true;
        else enemyActedThisRound = true;
        playerTurn = !playerTurn;

        if(playerActedThisRound && enemyActedThisRound) currentTurn++;
        StartCoroutine(StartTurnNextFrame());
    }
    #endregion
    #region Conquest
    public void CheckForGameEnd()
    {
        if (enemmyConquest <= 0)
        {
            Debug.Log("==PLAYER WIN===");
        }
        else if (playerConquest <= 0)
        {
            Debug.Log("==ENEMY WIN===");
        }
    }
    public void UnitScore(UnitOwner owner, bool died) //Si ha muerto suma al rival, sino resta al rival
    {
        if(died)
        {
            if (owner == UnitOwner.Enemy)
            {
                playerConquest++;
                enemmyConquest = Math.Min(enemmyConquest, gridSystem.Height - playerConquest);
            }
            else
            {
                enemmyConquest++;
                playerConquest = Math.Min(playerConquest, gridSystem.Height - playerConquest);
            }
        }
        else
        {
            if (owner == UnitOwner.Enemy) playerConquest--;
            else enemmyConquest--;
        }
    }
    void UpdateConquestTiles()
    {
        Tile[,] tiles = gridSystem.GetAllTiles();
        int width = gridSystem.Width;
        int height = gridSystem.Height;

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                Tile tile = tiles[x, z];

                if (z < enemmyConquest)
                {
                    tile.SetOwner(TileOwner.Enemy);
                }
                else if (z >= height - playerConquest)
                {
                    tile.SetOwner(TileOwner.Player);
                }
                else
                {
                    tile.SetOwner(TileOwner.Neutral);
                }
            }
        }
        enemyPlane.position = Vector3.back * (enemmyConquest + 0.5f);
        playerPlane.position = Vector3.back * ((height - playerConquest) + 0.5f);
    }
    #endregion
    public void EndPlacementPhase()
    {
        EndTurn();
    }
    public void RegisterPlacedUnit(Unit unit) //Afegir unitat a unitats activas
    {
        if (unit.GetOwner() == UnitOwner.Player) ActivePlayerUnits.Add(unit);
        else ActiveEnemyUnits.Add(unit);
    }
    System.Collections.IEnumerator StartTurnNextFrame()
    {
        yield return null;
        StartTurn();
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