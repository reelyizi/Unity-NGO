using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.UIElements;

public class PlayerMovement : NetworkBehaviour
{
    [SerializeField] private float speed = 3;
    [SerializeField] private Vector3 input;
    private CharacterController characterController;
    private Animator animator;

    [SerializeField] private NetworkVariable<PlayerStates> networkPlayerState = new NetworkVariable<PlayerStates>();

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (IsOwner && IsClient)
        {
            MovePlayer();
            NetworkUIManager.Instance.UpdatePing((int)NetworkManager.Singleton.NetworkConfig.NetworkTransport.GetCurrentRtt(NetworkManager.Singleton.LocalClientId));
        }

       
    }

    private void MovePlayer()
    {
        input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        if (input == Vector3.zero)
        {
            UpdateClientVisualServerRpc(PlayerStates.Idle);
            return;
        }

        Vector3 direction = input.normalized;


        UpdateClientPositionServerRpc(direction);
        UpdateClientRotationServerRpc(direction);
    }


    [ServerRpc]
    public void UpdateClientPositionServerRpc(Vector3 position)
    {
        UpdateClientVisualServerRpc(PlayerStates.Walking);        
        characterController.Move(position * Time.fixedDeltaTime * speed);
    }

    [ServerRpc]
    public void UpdateClientRotationServerRpc(Vector3 direction)
    {
        transform.LookAt(transform.position + direction);
    }

    [ServerRpc]
    public void UpdateClientVisualServerRpc(PlayerStates state)
    {
        //animator.SetTrigger($"{state}");
        GetComponent<OwnerNetworkAnimator>().SetTrigger($"{state}");
    }
}
