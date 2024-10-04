using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;
using System.Runtime.InteropServices.WindowsRuntime;

public class Moving : NetworkBehaviour
{
    public float moveSpeed = 5f;
    private CharacterController _characterController;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        if (!base.IsOwner)
            return;

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 offset = new Vector3(horizontal, Physics.gravity.y, vertical) * Time.deltaTime;

        _characterController.Move(offset);
    }
}
