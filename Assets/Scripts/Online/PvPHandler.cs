using Unity.Netcode;
using UnityEngine;

public class PvPHandler : NetworkBehaviour
{
    public static PvPHandler instance;
    private void Awake()
    {
        instance = this;
    }

    // --- INICIO DE PARTIDA ---
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
        BattleController.instance.StartPvPGame();
    }
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
        bool amIHost = IsServer;

        // Coordenadas finales que aplicaremos localmente
        int finalX = x;
        int finalY = y;

        // EL TRUCO DEL ESPEJO:
        // Si yo soy el Host y la acción NO es mía (es del cliente)...
        // O si yo soy el Cliente y la acción NO es mía (es del host)...
        // Significa que la unidad aparece en mi lado "Enemigo" (arriba), así que invierto coordenadas.

        if ((amIHost && !isHostAction) || (!amIHost && isHostAction))
        {
            // Invertimos X e Y basándonos en el tamaño total del mapa
            finalX = (GridSystem.instance.Width - 1) - x;
            finalY = (GridSystem.instance.Height - 1) - y;
        }

        // Ejecutamos la colocación física en el tablero local
        UnitPlacementManager.instance.ExecuteNetworkPlacement(unitIndex, finalX, finalY, isHostAction);
    }
}