using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MoveController
{
    private Transform playerTransform;
    private CharacterController characterController;
    private float moveSpeed = 10f;

    public MoveController(Transform movedTransform)
    {
        this.playerTransform = movedTransform;
        characterController = this.playerTransform.GetComponent<CharacterController>();
    }

    /// <summary>
    /// Moved the the player in direction of the absolute move-input deltas.
    /// </summary>
    public void Move(float x, float z, float time)
    {
        Vector3 moveValue = playerTransform.right * x + playerTransform.forward * z;
        moveValue *= time * moveSpeed;

        characterController.Move(moveValue);

    }
}
