using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class NetworkUI : MonoBehaviour
{
    public Button hostButton;
    public Button joinButton;
    public GameObject connectionPanel;

    void Start()
    {
        hostButton.onClick.AddListener(() => {
            NetworkManager.Singleton.StartHost();
            connectionPanel.SetActive(false);

            GameObject handler = Instantiate(NetworkManager.Singleton.NetworkConfig.Prefabs.Prefabs[0].Prefab);
            handler.GetComponent<NetworkObject>().Spawn();

            Invoke(nameof(StartGameDelayed), 1.0f);
        });

        joinButton.onClick.AddListener(() => {
            NetworkManager.Singleton.StartClient();
            connectionPanel.SetActive(false);
        });
    }

    void StartGameDelayed()
    {
        // Solo el host llama a esto
        PvPHandler.instance.StartGame();
    }
}