using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class CharacterSelectReady : NetworkBehaviour {
    public static CharacterSelectReady Instance {get; private set;}

    public event EventHandler OnReadyChanged;

    private Dictionary<ulong, bool> playerReadyDictionary;
    private float gameDurationInSeconds;

    private void Awake() {
        Instance = this;

        playerReadyDictionary = new Dictionary<ulong, bool>();
    }

    public void SetPlayerReady() {
        SetPlayerReadyServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    private void SetPlayerReadyServerRpc(ServerRpcParams serverRpcParams = default) {
        SetPlayerReadyClientRpc(serverRpcParams.Receive.SenderClientId);

        playerReadyDictionary[serverRpcParams.Receive.SenderClientId] = true;

        bool allClientsReady = true;
        foreach(ulong clientId in NetworkManager.Singleton.ConnectedClientsIds) {
            if (!playerReadyDictionary.ContainsKey(clientId) || !playerReadyDictionary[clientId]) {
                // This player is not ready
                allClientsReady = false;
                break;
            }
        }

        if(allClientsReady) {
            KitchenGameMultiplayer.Instance.SetGameDurationInSeconds(gameDurationInSeconds);
            KitchenGameLobby.Instance.DeleteLobby();

            Loader.LoadNetwork(Loader.Scene.GameScene);
        }
    }

    [ClientRpc]
    private void SetPlayerReadyClientRpc(ulong clientId) {
        playerReadyDictionary[clientId] = true;

        OnReadyChanged?.Invoke(this, EventArgs.Empty);
    }

    public bool IsPlayerReady(ulong clientId) {
        return playerReadyDictionary.ContainsKey(clientId) && playerReadyDictionary[clientId];
    }    

    public void SetGameDurationInSeconds(float gameDurationInSeconds) {
        this.gameDurationInSeconds = gameDurationInSeconds;
    }
}
