using UnityEngine;
using static BattleController;

public class PlayerPlacementModule : EnemyModule
{
    PlacementEnded onFinished;
    bool waitingInput;
    Unit pendingPrefab;

    [Header("Raycast")]
    [SerializeField] LayerMask tileLayerMask = ~0;

    bool finishedThisPhase;

    public override void OnPlacementPhase(PlacementEnded onFinished)
    {
        finishedThisPhase = false;
        this.onFinished = onFinished;

        if (!battle.TryPeekUnitToPlace(true, out Unit unitPrefab))
        {
            waitingInput = false;
            pendingPrefab = null;

            if (!finishedThisPhase)
            {
                finishedThisPhase = true;
                this.onFinished?.Invoke();
            }
            return;
        }

        pendingPrefab = unitPrefab;
        waitingInput = true;
    }



    void Update()
    {
        if (!waitingInput) return;

        if (Input.GetMouseButtonDown(0))
        {
            Tile tile = RaycastTileUnderMouse();
            if (tile == null) return;

            if (!battle.CanPlaceOn(tile, isPlayer: true))
            {
                Debug.Log("No puedes colocar ahí, no es tuya o está ocupada.");
                return;
            }

            battle.PlaceUnitOn(tile, pendingPrefab, isPlayer: true);
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

    Tile RaycastTileUnderMouse()
    {
        Camera cam = Camera.main;
        if (cam == null) return null;

        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 200f, tileLayerMask))
        {
            return hit.collider.GetComponent<Tile>() ?? hit.collider.GetComponentInParent<Tile>();
        }
        return null;
    }
}

