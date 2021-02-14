using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HealthBehaviour : MonoBehaviour
{
    [SerializeField]
    private int Hitpoints = 100;

    [SerializeField]
    private UnityEvent DestroyedOrKilled;

    public void Damage(int amount, out bool destroyOrKill)
    {
        Hitpoints -= amount;
        if (Hitpoints < 0)
            Hitpoints = 0;
        destroyOrKill = Hitpoints == 0;
        Debug.Log($"HP: {Hitpoints}");
        if (destroyOrKill)
            DestroyedOrKilled?.Invoke();
    }
}
