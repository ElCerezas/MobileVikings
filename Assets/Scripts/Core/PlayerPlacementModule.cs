using UnityEngine;
using static BattleController;

public class PlayerPlacementModule : MonoBehaviour
{
    BattleController battle;
    PlacementEnded onFinished;
    bool waitingInput;
    public Unit pendingPrefab;

    bool finishedThisPhase;

    public void Initialize(BattleController controller)
    {
        battle = controller;
    }
    public void OnPlacementPhase(PlacementEnded onFinished)
    {
        finishedThisPhase = false;
        this.onFinished = onFinished;



        //pendingPrefab = unitPrefab;
        waitingInput = true;
    }
    public void SelectUnit(GameObject unitToSummon)
    {
        pendingPrefab = unitToSummon.GetComponent<Unit>();
    }
    public void TryPlaceUnit(Tile targetTile)
    {
        if (waitingInput)
        {
            if (!battle.CanPlaceOn(targetTile, isPlayer: true))
            {
                Debug.Log("No puedes colocar ahí, no es tuya o está ocupada.");
                return;
            }

            battle.PlaceUnitOn(targetTile, pendingPrefab, isPlayer: true);
            battle.ConsumeUnitToPlace(true, pendingPrefab);

            waitingInput = false;
            pendingPrefab = null;

            if (!finishedThisPhase)
            {
                finishedThisPhase = true;
                onFinished?.Invoke();
            }
        }
    }
}

