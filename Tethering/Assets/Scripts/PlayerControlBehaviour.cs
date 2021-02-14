using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerControlBehaviour : MonoBehaviour
{
    [SerializeField]
    private AnimationCurve _velocityCurve;
    [SerializeField]
    private float _velocityMultiplier = 5f;
    [SerializeField]
    private GravityObject _gravytyObject;

    private bool _isControlling = false;

    private Plane _zeroPlane = new Plane(Vector3.up, Vector3.zero);

    private Rigidbody _body;

    private void Awake()
    {
        _body = GetComponent<Rigidbody>();
    }

    private void OnDisable()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
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

        _gravytyObject.GravityCenter = transform.position;
    }

    private void ProcessInput()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (_zeroPlane.Raycast(ray, out float distance))
        {
            var point = ray.GetPoint(distance);
            var relativePoint = transform.position - point;
            var relativeDistance = relativePoint.magnitude;
            var inverseDirection = -relativePoint.normalized;
            _body.velocity = new Vector3(
                _velocityCurve.Evaluate(inverseDirection.x),
                0f,
                _velocityCurve.Evaluate(inverseDirection.z)
                )
                * _velocityMultiplier * relativeDistance;
        }
    }
}
