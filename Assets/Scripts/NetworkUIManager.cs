using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class NetworkUIManager : NetworkSingleton<NetworkUIManager>
{
    [SerializeField] private Button HostButton;
    [SerializeField] private Button ClientButton;
    [SerializeField] private Button ServerButton;
    [SerializeField] private Button ExecutePhysicsButton;

    [SerializeField] TextMeshProUGUI playersInGameText;
    [SerializeField] TextMeshProUGUI playerPing;

    private bool hasServerStarted;

    private void Start()
    {
        HostButton.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartHost();
        });
        ClientButton.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartClient();
        });
        ServerButton.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartServer();
        });

        NetworkManager.Singleton.OnServerStarted += () =>
        {
            hasServerStarted = true;
        };

        ExecutePhysicsButton.onClick.AddListener(() =>
        {
            if (!hasServerStarted) return;

            
            SpawnerControl.Instance.SpawnObjects();
        });

        NetworkManager.Singleton.OnClientConnectedCallback += (id) =>
        {
            Logger.Instance.LogInfo($"{id} is connected");
        };
    }

    private void Update()
    {
        playersInGameText.text = $"Players in game: {PlayersManager.Instance.PlayerInGame}";
    }

    public void UpdatePing(int ping)
    {
        playerPing.text = "Ping: " + ping;
    }
}
