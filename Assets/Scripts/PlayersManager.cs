using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayersManager : NetworkSingleton<PlayersManager>
{
    private NetworkVariable<int> playersInGame = new NetworkVariable<int>();

    public int PlayerInGame
    {
        get { return playersInGame.Value; }
    }

    private void Start()
    {
        NetworkManager.Singleton.OnClientConnectedCallback += (id) =>
        {
            if (IsHost)
            {
                playersInGame.Value++;
            }
        };

        NetworkManager.Singleton.OnClientDisconnectCallback += (id) =>
        {
            if (IsHost)
            {
                playersInGame.Value--;
            }
        };
    }
}
