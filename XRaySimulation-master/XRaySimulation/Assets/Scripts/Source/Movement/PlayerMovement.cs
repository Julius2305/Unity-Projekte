using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private MoveController moveController;

    private void Start()
    {
        moveController = new MoveController(this.transform);
    }

    private void FixedUpdate()
    {
        float x = UnityInput.Instance.GetAxis("Horizontal Movement");
        float z = UnityInput.Instance.GetAxis("Vertical Movement");

        moveController.Move(x, z, Time.fixedDeltaTime);
    }
}
