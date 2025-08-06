using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;
using UnityEngine;

public class InputController: IDisposable
{
    private readonly InputControls _InputActions;
    public InputControls InputActions => _InputActions;

    private IDisposable _eventListener;
    public event Action<Vector2> MovementRecieved;  
    public event Action MovementEnded;
    public event Action EscapeButtonPressed;  
    public void Dispose()
    {
        // Unsubscribe all events or clean up resources here
        MovementRecieved = null;
    }

    public InputController()
    {
        _InputActions = new InputControls();
        _InputActions.Enable();
    }

    public void SubscribeEvents()
    {
        _InputActions.Default.Movement.performed += OnMovementPerformed;
        _InputActions.Default.ExitButton.performed += OnEscapePerformed;
        _InputActions.Default.Movement.canceled += OnMovementEnded;
    }

    public void UnsubscribeEvents()
    {
        _InputActions.Default.Movement.performed -= OnMovementPerformed;
        _InputActions.Default.ExitButton.performed -= OnEscapePerformed;
        _InputActions.Default.Movement.canceled -= OnMovementEnded;
    }

    private void OnMovementPerformed(InputAction.CallbackContext callbackContext) => MovementRecieved?.Invoke(callbackContext.ReadValue<Vector2>());
    private void OnMovementEnded(InputAction.CallbackContext callbackContext) => MovementEnded?.Invoke();

    private void OnEscapePerformed(InputAction.CallbackContext callbackContext) => EscapeButtonPressed?.Invoke();


    public void OnDestroy()
    {
        UnsubscribeEvents();
        _InputActions.Dispose();
    }
}