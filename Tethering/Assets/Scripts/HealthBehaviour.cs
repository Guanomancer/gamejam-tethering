using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class HealthBehaviour : MonoBehaviour
{
    [SerializeField]
    private int Hitpoints = 100;

    [SerializeField]
    private UnityEvent DamageTaken;

    [SerializeField]
    private UnityEvent DestroyedOrKilled;

    [SerializeField]
    private GameObject SpawnOnDestroyOrKilled;

    [SerializeField]
    private Transform _textField;

    public void Damage(int amount, out bool destroyOrKill)
    {
        Hitpoints -= amount;
        if (Hitpoints < 0)
            Hitpoints = 0;
        destroyOrKill = Hitpoints == 0;
        if (destroyOrKill)
        {
            DestroyedOrKilled?.Invoke();
            if (SpawnOnDestroyOrKilled != null)
                Instantiate(SpawnOnDestroyOrKilled, transform.position, transform.rotation);
        }
        else
            DamageTaken?.Invoke();

        UpdateTextField();
    }

    private void UpdateTextField()
    {
        if (_textField != null)
        {
            var text = _textField.GetComponent<TextMeshProUGUI>();
            text.text = $"HP: {Hitpoints}";
        }
    }

    private void OnEnable()
    {
        UpdateTextField();
    }
}
