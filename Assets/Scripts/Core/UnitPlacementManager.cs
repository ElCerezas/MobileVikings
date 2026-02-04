using System.Collections.Generic;
using UnityEngine;
using static BattleController;
using Unity.Netcode;

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
            unit.controller.TurnCollider(true);
            UpdateDeckVisuals(playerDeck, playerDeckOrigin, Vector3.right);
        }
        else
        {
            enemyDeck.Add(unit);
            UpdateDeckVisuals(enemyDeck, enemyDeckOrigin, Vector3.right);
        }
    }
    public void UpdateDeckVisuals(List<Unit> deck, Transform origin, Vector3 direction) //Gepeteada de manual, quin pal escriure lol
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
        if (unit.GetOwner() == UnitOwner.Player)
        {
            unit.Placement(tile, 1);
        }
        else
        {
            unit.Placement(tile, 1);
        }
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
    /*Old script
     * public void PlayerTryPlace(Tile targetTile)
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
    }*/
    public void PlayerTryPlace(Tile targetTile)
    {
        if (!canPlaceUnit || selectedUnit == null) return;

        // REGLA DE ORO: Para ambos, su zona es la de abajo (TileOwner.Player)
        // porque visualmente el tablero está configurado igual para los dos.
        if (targetTile.owner != TileOwner.Player)
        {
            Debug.Log("Solo puedes colocar en tu zona (abajo)");
            return;
        }

        bool amIHost = NetworkManager.Singleton.IsServer;

        // Validar turno
        if (BattleController.instance.playerTurn != amIHost) return;

        // Buscamos la unidad en el mazo (Cada uno usa su playerDeck local)
        int index = playerDeck.IndexOf(selectedUnit);

        if (index != -1)
        {
            // Enviamos las coordenadas TAL CUAL las vemos en nuestra pantalla
            PvPHandler.instance.TryPlaceUnit(index, targetTile.x, targetTile.y);

            canPlaceUnit = false;
            DeselectUnit();
        }
    }
    public void ExecuteNetworkPlacement(int unitIndex, int x, int y, bool isHostAction)
    {
        Tile targetTile = GridSystem.instance.GetTile(x, y);
        bool amIHost = NetworkManager.Singleton.IsServer;

        Unit unitToPlace = null;
        List<Unit> deckToUse;

        // ¿La unidad es mía o del rival?
        bool isMyUnit = (amIHost == isHostAction);

        if (isMyUnit)
        {
            deckToUse = playerDeck; // Mi mazo (abajo)
        }
        else
        {
            deckToUse = enemyDeck; // Mazo rival (arriba)
        }

        if (unitIndex < deckToUse.Count)
        {
            unitToPlace = deckToUse[unitIndex];

            // Lógica de spawn
            unitToPlace.transform.position = targetTile.transform.position + (Vector3.up * 0.5f);
            targetTile.SetNewOccupant(unitToPlace);

            // Registrar según quién la puso (para que el combate sepa de quién es)
            //unitToPlace.(isMyUnit ? UnitOwner.Player : UnitOwner.Enemy);
            BattleController.instance.RegisterPlacedUnit(unitToPlace);

            // Limpiar mazo visual
            deckToUse.Remove(unitToPlace);
            UpdateDeckVisuals(deckToUse, isMyUnit ? playerDeckOrigin : enemyDeckOrigin, Vector3.right);

            BattleController.instance.EndPlacementPhase();
        }
    }
    #endregion
    #region Enemy Placement
    public void PlaceEnemyUnit(Unit unit, Tile targetTile)
    {
        //ExecutePlacement(unit, targetTile);

        //enemyDeck.Remove(unit);
        //UpdateDeckVisuals(enemyDeck, enemyDeckOrigin, Vector3.right);
    }
    public List<Unit> GetEnemyDeck()
    {
        return enemyDeck;
    }
    #endregion

}

