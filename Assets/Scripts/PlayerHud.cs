using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.UIElements.Experimental;
using Unity.Collections;
using TMPro;
using System;

public class PlayerHud : NetworkBehaviour
{
    private NetworkVariable<FixedString64Bytes> playersName = new NetworkVariable<FixedString64Bytes>();

    private bool overlaySet = false;

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            playersName.Value = $"Player {OwnerClientId}";
        }
    }

    public void SetOverlay()
    {
        var localPlayerOverlay = gameObject.GetComponentInChildren<TextMeshProUGUI>();
        localPlayerOverlay.text = Convert.ToString(playersName.Value);
    }

    private void Update()
    {
        if(!overlaySet && !string.IsNullOrEmpty(Convert.ToString(playersName.Value)))
        {
            SetOverlay();
            overlaySet = true;
        }
    }
}