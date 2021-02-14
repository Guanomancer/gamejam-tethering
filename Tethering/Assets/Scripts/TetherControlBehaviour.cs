using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class TetherControlBehaviour : MonoBehaviour
{
    [SerializeField]
    private float _gravityStrength = 5f;
    [SerializeField]
    private float _maxVelocity = 1f;

    [SerializeField]
    private Transform _targetObject;

    private Plane _zeroPlane = new Plane(Vector3.up, Vector3.zero);

    private Rigidbody _body;

    private void Awake()
    {
        _body = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        ProcessTether();

        var velocity = _body.velocity.magnitude;
        if(velocity > _maxVelocity)
        {
            var direction = _body.velocity.normalized;
            _body.velocity = direction * _maxVelocity;
        }
    }

    private void ProcessTether()
    {
        var point = _targetObject.position;
        var relativePoint = transform.position - point;
        var inverseDirection = -relativePoint.normalized;
        Physics.gravity = inverseDirection * _gravityStrength;
    }
}
