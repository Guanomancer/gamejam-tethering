using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Gravity Object")]
public class GravityObject : ScriptableObject
{
    public Vector3 GravityCenter = Vector3.zero;

    public void ProcessTether(GameObject tether, Rigidbody body, float gravityStrength)
    {
        var point = GravityCenter;
        var relativePoint = tether.transform.position - point;
        var inverseDirection = -relativePoint;
        body.AddForce(inverseDirection * gravityStrength * Time.deltaTime);
    }
}
