using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerMovement : NetworkBehaviour
{
    [SerializeField] private float speed = 3;
    [SerializeField] private Vector3 input;
    private Rigidbody rigidbody;
    private CharacterController characterController;


    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        MovePlayer();
    }

    private void MovePlayer()
    {
        input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        if (input == Vector3.zero) return;
        Vector3 newPosition = input.normalized * speed;
        characterController.Move(newPosition * Time.fixedDeltaTime);
        //rigidbody.MovePosition(newPosition);        
        //transform.position += (input.normalized * speed * Time.deltaTime);


    }
}
