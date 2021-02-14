using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PointBehaviour : MonoBehaviour
{
    public static int Score = 0;

    [SerializeField]
    private int _points = 50;

    public TextMeshProUGUI PointsTextField;

    private void OnTriggerEnter(Collider other) => OnEnter(other.transform);
    private void OnCollisionEnter(Collision collision) => OnEnter(collision.transform);

    private void OnEnter(Transform transform)
    {
        Score += _points;
        UpdatePointText();
    }

    private void UpdatePointText()
    {
        var text = PointsTextField.GetComponent<TextMeshProUGUI>();
        text.text = $"Points: {Score}";
    }
}
