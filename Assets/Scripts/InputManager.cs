using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }

     public static event Action OnHoldStarted;
     public static event Action OnHoldCanceled;
    
    
    [SerializeField] private InputActionAsset _generalInputSystem;

    private void Awake()
    {
        if(Instance != null) Destroy(Instance);
        Instance = this;
    }

    private void Start()
    {
        _generalInputSystem.Enable();
        _generalInputSystem.actionMaps[1].Enable();
        _generalInputSystem.actionMaps[1].actions[10].Enable(); // Hold action
        _generalInputSystem.actionMaps[1].actions[10].started += OnHoldBegins;
        _generalInputSystem.actionMaps[1].actions[10].canceled += OnHoldFinished;
        
    }

    private void OnHoldBegins(InputAction.CallbackContext obj)
    {
        OnHoldStarted?.Invoke();
    }
    private void OnHoldFinished(InputAction.CallbackContext obj)
    {
        OnHoldCanceled?.Invoke();
    }

    
}
