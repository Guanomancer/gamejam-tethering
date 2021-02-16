using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraBehaviour : MonoBehaviour
{
    [SerializeField]
    private float _widthScale = 50f;

    [SerializeField]
    private AnimationCurve _scalingCurve;

    [SerializeField]
    private float _moveTime = 2.5f;

    [SerializeField]
    private float _panRange = 20f;

    private Plane _zeroPlane = new Plane(Vector3.up, Vector3.zero);

    private float _startY;
    private float _endY;
    private float _startTime;

    private Camera _camera;

    private void Awake()
    {
        _camera = GetComponent<Camera>();

        _startY = transform.position.y;
        _startTime = Time.time;
    }

    private void Update()
    {
        var zero = _camera.ScreenToWorldPoint(Vector3.zero);
        var one = _camera.ScreenToWorldPoint(new Vector3(Screen.width, 0f, Screen.height));
        var ratio = Mathf.Abs(one.z) / Mathf.Abs(one.x);
        _endY = ratio * _widthScale;

        var pos = transform.position;
        pos.y = Mathf.Lerp(_startY, _endY, _scalingCurve.Evaluate((Time.time - _startTime) / _moveTime));
        transform.position = pos;

        var screenOffset = new Vector3(Screen.width / 2, Screen.height / 2, 0);
        var centeredMousePosition = Input.mousePosition - screenOffset;
        centeredMousePosition = new Vector3(centeredMousePosition.x, centeredMousePosition.z, centeredMousePosition.y);
        centeredMousePosition = new Vector3(
            centeredMousePosition.x / Screen.width,
            0,
            centeredMousePosition.z / Screen.height);
        centeredMousePosition *= 2f;

        pos = transform.position;
        pos.x = _panRange * centeredMousePosition.x;
        pos.z = _panRange * centeredMousePosition.z;
        transform.position = pos;
    }
}
