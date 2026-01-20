using System.Collections.Generic;
using UnityEngine;
using static BattleController;
using static UnityEngine.UI.GridLayoutGroup;

public class UnitPlacementManager : MonoBehaviour
{
    [Header("Core")]
    [SerializeField]List<Unit> playerDeck = new List<Unit>();
    [SerializeField]List<Unit> enemyDeck = new List<Unit>();
    BattleController battleController;
    public static UnitPlacementManager instance;


    [Header("Configuració Visual")]
    [SerializeField] private Transform playerDeckOrigin;
    [SerializeField] private Transform enemyDeckOrigin;
    [SerializeField] private float slotSpacing = 1.5f;

    [Header("PlayerManager")]
    [SerializeField] Unit selectedUnit;
    bool canPlaceUnit = false;
    PlacementEnded finished;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        battleController = BattleController.instance;
    }
    public void ReturnToDeck(Unit unit)
    {
        UnitOwner owner = unit.GetOwner();
        if (owner == UnitOwner.Player) BattleController.instance.ActivePlayerUnits?.Remove(unit);
        else BattleController.instance.ActiveEnemyUnits?.Remove(unit);

        if (owner == UnitOwner.Player)
        {
            playerDeck.Add(unit);
            UpdateDeckVisuals(playerDeck, playerDeckOrigin, Vector3.right);
        }
        else
        {
            enemyDeck.Add(unit);
            UpdateDeckVisuals(enemyDeck, enemyDeckOrigin, Vector3.right);
        }
    }
    private void UpdateDeckVisuals(List<Unit> deck, Transform origin, Vector3 direction) //Gepeteada de manual, quin pal escriure lol
    {
        for (int i = 0; i < deck.Count; i++)
        {
            deck[i].transform.position = origin.position + (direction * (i * slotSpacing));
            deck[i].transform.rotation = Quaternion.identity;
        }
    }

    bool CanPlaceOn(Tile tile, bool isPlayer) 
    {
        if (tile == null) return false;
        if (!tile.IsFree) return false;
        if (isPlayer && tile.owner != TileOwner.Player) return false;
        if (!isPlayer && tile.owner != TileOwner.Enemy) return false;
        return true;
    }
    void ExecutePlacement(Unit unit, Tile tile)
    {
        unit.transform.position = tile.transform.position + (Vector3.up / 2);

        tile.SetNewOccupant(unit);
        unit.Placement(tile, 1);
        battleController.RegisterPlacedUnit(unit);
        if(finished != null) {
            finished?.Invoke();
            finished = null;
        }
    }
    #region Player Placement
    public void OnPlayerPlacementPhase(PlacementEnded onFinished)
    {
        canPlaceUnit = true;
        finished = onFinished;
    }
    public void OnUnitSelected(Unit unit)
    {
        if (selectedUnit == unit)
        {
            DeselectUnit();
            Debug.Log("Unidad deseleccionada");
        }
        else
        {
            selectedUnit = unit;
            Debug.Log($"Unidad seleccionada: {unit.name}");
        }
    }
    public void DeselectUnit()
    {
        selectedUnit = null;
    }
    public void PlayerTryPlace(Tile targetTile)
    {
        if (!canPlaceUnit) return;
        if (selectedUnit == null) return;

        if (CanPlaceOn(targetTile, true))
        {
            canPlaceUnit = false;
            ExecutePlacement(selectedUnit, targetTile);

            // Limpieza
            playerDeck.Remove(selectedUnit);
            UpdateDeckVisuals(playerDeck, playerDeckOrigin, Vector3.right);
            DeselectUnit();
        }
        else
        {
            Debug.Log("Colocación inválida.");
        }
    }
    #endregion
    #region Enemy Placement
    public void PlaceEnemyUnit(Unit unit, Tile targetTile)
    {
        ExecutePlacement(unit, targetTile);

        enemyDeck.Remove(unit);
        UpdateDeckVisuals(enemyDeck, enemyDeckOrigin, Vector3.right);
    }
    #endregion
    
}

