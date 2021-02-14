using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class TetherControlBehaviour : MonoBehaviour
{
    [SerializeField]
    private float _gravityStrength = 5f;
    //[SerializeField]
    //private float _velocityTimeScale = 1f;
    [SerializeField]
    private float _maxVelocity = 10f;

    [SerializeField]
    private GravityObject _gravityObject;

    [SerializeField]
    private Transform _targetObject;

    [SerializeField]
    private float _rotatyness = 1;

    private Plane _zeroPlane = new Plane(Vector3.up, Vector3.zero);

    private Rigidbody _body;

    public void SetTarget(Transform newTarget) => _targetObject = newTarget;

    private void Awake()
    {
        _body = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if(_targetObject != null)
            ProcessTether();

        var velocity = _body.velocity.magnitude;
        if(velocity > _maxVelocity)
        {
            var direction = _body.velocity.normalized;
            _body.velocity = direction * _maxVelocity;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        var tether = collision.transform.GetComponent<TetherControlBehaviour>();
        if (tether != null)
        {
            if (_targetObject == null)
                SetTarget(tether._targetObject);
            else
                _body.angularVelocity += Random.insideUnitSphere * _rotatyness;
        }
    }

    private void ProcessTether()
    {
        var point = _gravityObject.GravityCenter;
        var relativePoint = transform.position - point;
        var inverseDirection = -relativePoint;
        _body.AddForce(inverseDirection * _gravityStrength * Time.deltaTime);
    }
}
