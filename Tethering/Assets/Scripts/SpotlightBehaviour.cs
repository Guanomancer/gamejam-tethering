using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpotlightBehaviour : MonoBehaviour
{
    private void Update()
    {
        var ray = Camera.main.ScreenPointToRay(Camera.main.WorldToScreenPoint(transform.position));
        transform.rotation = Quaternion.LookRotation(ray.direction);
    }
}
