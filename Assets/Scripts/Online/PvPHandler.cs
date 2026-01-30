using Unity.Netcode;
using UnityEngine;

public class PvPHandler : NetworkBehaviour
{
    public static PvPHandler instance;

    private void Awake()
    {
        instance = this;
    }

    // --- FASE 1: INICIO DE PARTIDA ---
    public void StartGame()
    {
        if (IsServer)
        {
            int seed = Random.Range(0, 999999);
            StartGameClientRpc(seed);
        }
    }

    [ClientRpc]
    private void StartGameClientRpc(int seed)
    {
        Random.InitState(seed);
        Debug.Log($"Partida iniciada. Seed sincronizada: {seed}");

        BattleController.instance.StartPvPGame();
    }


    // --- FASE 2: COLOCACIÓN DE UNIDADES ---
    public void TryPlaceUnit(int unitIndex, int x, int y)
    {
        if (IsServer)
        {
            ReceivePlacementClientRpc(unitIndex, x, y, true);
        }
        else
        {
            RequestPlacementServerRpc(unitIndex, x, y);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void RequestPlacementServerRpc(int unitIndex, int x, int y)
    {
        ReceivePlacementClientRpc(unitIndex, x, y, false);
    }

    [ClientRpc]
    private void ReceivePlacementClientRpc(int unitIndex, int x, int y, bool isHostAction)
    {
        UnitPlacementManager.instance.ExecuteNetworkPlacement(unitIndex, x, y, isHostAction);
    }
}