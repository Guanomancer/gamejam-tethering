using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Gravity Object")]
public class GravityObject : ScriptableObject
{
    public Vector3 GravityCenter = Vector3.zero;
}
