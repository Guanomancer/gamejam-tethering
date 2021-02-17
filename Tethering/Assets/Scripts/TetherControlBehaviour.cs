using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class TetherControlBehaviour : MonoBehaviour
{
    [SerializeField]
    private float _gravityStrength = 50f;

    [SerializeField]
    private float _maxVelocity = 5f;

    [SerializeField]
    private GravityObject _gravityObject;

    [SerializeField]
    private Transform _targetObject;

    [SerializeField]
    private float _rotatyness = 1;

    [SerializeField]
    private GameObject CollisionEffect;

    [SerializeField]
    private float _startSpeedMultiplier = 0.5f;
    [SerializeField]
    private float _maxSpeedMultiplier = 2f;
    [SerializeField]
    private float _speedMultiplierTimeScalar = 10f;

    public float GameTimeOffset = 0;

    private Plane _zeroPlane = new Plane(Vector3.up, Vector3.zero);

    private Rigidbody _body;

    public void SetTarget(Transform newTarget) => _targetObject = newTarget;

    private float ActualSpeedScalar { get => Mathf.Clamp(Mathf.Lerp(_startSpeedMultiplier, _maxSpeedMultiplier, (Time.time - GameTimeOffset) / _speedMultiplierTimeScalar), _startSpeedMultiplier, _maxSpeedMultiplier); }
    
    private void Awake()
    {
        _body = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (_targetObject != null)
            _gravityObject?.ProcessTether(gameObject, _body, _gravityStrength * ActualSpeedScalar);

        var actualMaxVelocity = _maxVelocity * ActualSpeedScalar;
        var velocity = _body.velocity.magnitude;
        if (velocity > actualMaxVelocity)
        {
            var direction = _body.velocity.normalized;
            _body.velocity = direction * actualMaxVelocity;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        var tether = collision.transform.GetComponent<TetherControlBehaviour>();
        if (tether != null)
        {
            if (_targetObject == null)
            {
                SetTarget(tether._targetObject);
                if (CollisionEffect != null)
                    Instantiate(CollisionEffect, transform.position, transform.rotation);
            }
            else
                _body.angularVelocity += Random.insideUnitSphere * _rotatyness;
        }
    }
}
