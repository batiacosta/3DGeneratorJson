using System;
using Unity.Cinemachine;
using UnityEngine;

public class CameraControllersManager : MonoBehaviour
{
    private CinemachineInputAxisController _axisController;
    private void Start()
    {
        _axisController = GetComponent<CinemachineInputAxisController>();
        EnableInputAxisController(false);
        InputManager.OnHoldStarted += OnHoldStarted;
        InputManager.OnHoldCanceled += OnHoldCanceled;
    }

    

    private void OnDestroy()
    {
        InputManager.OnHoldStarted += OnHoldStarted;
        InputManager.OnHoldCanceled += OnHoldCanceled;
    }

    private void OnHoldStarted()
    {
        EnableInputAxisController(true);
    }

    private void OnHoldCanceled()
    {
        EnableInputAxisController(false);
    }
    private void EnableInputAxisController(bool enabled)
    {
        _axisController.enabled = enabled;
    }
}
