using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ToggleButtonBehaviour : MonoBehaviour
{
    [SerializeField]
    private bool _isOn = false;

    [SerializeField]
    private UnityEvent OnActivate;

    [SerializeField]
    private UnityEvent OnDeactivate;

    public void Toggle()
    {
        _isOn = !_isOn;
        if (_isOn)
            OnActivate?.Invoke();
        else
            OnDeactivate?.Invoke();
    }
}
