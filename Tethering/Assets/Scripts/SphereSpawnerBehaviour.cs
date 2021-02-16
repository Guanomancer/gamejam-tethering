using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SphereSpawnerBehaviour : MonoBehaviour
{
    [SerializeField]
    private GameObject _spherePrefab;
    [SerializeField]
    private GameObject _playerObject;

    [SerializeField]
    private float _spawnInterval = 10f;

    [SerializeField]
    private float _spawnPlayerRadiusMin = 5f;
    [SerializeField]
    private float _spawnSceneRadiusMax = 20f;

    [SerializeField]
    private TextMeshProUGUI _pointsTextField;

    private float _nextSpawn = 0f;

    private void Awake()
    {
        _nextSpawn = Time.time + _spawnInterval;
    }

    private void Update()
    {
        if (_nextSpawn <= Time.time)
        {
            _nextSpawn = Time.time + _spawnInterval;
            SpawnSphere();
        }
    }

    private void SpawnSphere()
    {
        var randomPosition = Vector3.zero;
        var position = Vector3.zero;
        do
        {
            var randomCircle = Random.insideUnitCircle;
            randomPosition = new Vector3(randomCircle.x, 0f, randomCircle.y) * _spawnSceneRadiusMax;
            if (Vector3.Distance(randomPosition, _playerObject.transform.position) >= _spawnPlayerRadiusMin)
                position = randomPosition;
        } while (position == Vector3.zero);

        var sphere = Instantiate(_spherePrefab, position, Quaternion.identity);
        var points = sphere.GetComponent<PointBehaviour>();
        points.PointsTextField = _pointsTextField;
    }
}
