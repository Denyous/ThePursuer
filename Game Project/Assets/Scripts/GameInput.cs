using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    private GameInputActions inputActions;

    private void Awake()
    {
        inputActions = new GameInputActions();
        inputActions.Steer.Enable();
    }

    public Vector2 GetMovementVectorNormalized()
    {
        Vector2 inputVector = inputActions.Steer.Steer.ReadValue<Vector2>();

        inputVector = inputVector.normalized;

        return inputVector;
    }
}
