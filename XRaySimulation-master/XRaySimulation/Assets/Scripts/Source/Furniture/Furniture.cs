using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TestTools;

[RequireComponent(typeof(SphereCollider))]
public class Furniture : MonoBehaviour
{
    [SerializeField] public FurnitureType Type;
    [SerializeField] public FurnitureInfo Info;

    [HideInInspector] public FurnitureController Controller;

    private SphereCollider trigger;

    private bool[] instructions;

    private void Awake()
    {
        trigger = this.gameObject.GetComponent<SphereCollider>();
        trigger.isTrigger = true;

        this.Controller = FurnitureControllerFactory.Create(Type, this.transform);
    }

    private void Update()
    {
        if (Controller.isTriggerd)
        {
            GetInstructions();
            Controller.Interact(instructions, Time.deltaTime);
        }
    }

    /// <summary>
    /// Stores the user input instructions of this frame.
    /// Foreach KeyCode in FurnitureInfo.
    /// </summary>
    private void GetInstructions()
    {
        instructions = new bool[Info.KeyCodes.Length];

        for (int i = 0; i < Info.KeyCodes.Length; i++)
        {
            instructions[i] = GetInstructionBasedOnType(Info.KeyCodes[i]);
        }
    }

    
    /// <summary>
    /// Checks the user input for the given KeyCode.
    /// </summary>
    private bool GetInstructionBasedOnType(KeyCode code)
    {
        // there should be a better way
        // is needed because GetKey will fire each frame
        // this leads to interaction with the furniture each frame
        // resulting in activating/deactivating it multiple times in a single press
        // Which is only needed for the patient table.

        if (Type == FurnitureType.PatientTable)
            return UnityInput.Instance.GetKey(code);
        else
            return UnityInput.Instance.GetKeyDown(code);
    }

    private void OnTriggerEnter(Collider other)
    {
        Controller.Activate(Type, Info);
    }

    private void OnTriggerExit(Collider other)
    {
        Controller.Deactivate();
    }
}
