using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifetimeBehaviour : MonoBehaviour
{
    [SerializeField]
    private float _lifeTime = 1f;

    private void OnEnable()
    {
        Destroy(gameObject, _lifeTime);
    }
}
