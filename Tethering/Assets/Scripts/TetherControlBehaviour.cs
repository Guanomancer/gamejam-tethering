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

    private bool _isControlling = false;

    private Plane _zeroPlane = new Plane(Vector3.up, Vector3.zero);

    private Rigidbody _body;

    private void Awake()
    {
        _body = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if(Input.GetMouseButton(0))
        {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
            _isControlling = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            _isControlling = false;
            Physics.gravity = Vector3.zero;
        }
    }

    private void FixedUpdate()
    {
        if (_isControlling)
            ProcessInput();

        var velocity = _body.velocity.magnitude;
        if(velocity > _maxVelocity)
        {
            var direction = _body.velocity.normalized;
            _body.velocity = direction * _maxVelocity;
        }
    }

    private void ProcessInput()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(_zeroPlane.Raycast(ray, out float distance))
        {
            var point = ray.GetPoint(distance);
            var relativePoint = transform.position - point;
            var inverseDirection = -relativePoint.normalized;
            Physics.gravity = inverseDirection * _gravityStrength;
        }
    }
}
