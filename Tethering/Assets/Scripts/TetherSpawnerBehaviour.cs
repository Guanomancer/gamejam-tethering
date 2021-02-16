using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TetherSpawnerBehaviour : MonoBehaviour
{
    [SerializeField]
    private GameObject _tetherPrefab;
    [SerializeField]
    private GameObject _playerObject;
    [SerializeField]
    private bool _setTargetAtSpawn = false;

    [SerializeField]
    private float _spawnInterval = 10f;

    [SerializeField]
    private float _spawnPlayerRadiusMin = 5f;
    [SerializeField]
    private float _spawnSceneRadiusMax = 20f;

    [SerializeField]
    private GameBehaviour _gameBehaviour;

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
            SpawnTether();
        }
    }

    private void SpawnTether()
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

        var obj = Instantiate(_tetherPrefab, position, Quaternion.identity);
        var tether = obj.GetComponent<TetherControlBehaviour>();
        tether.GameTimeOffset = _gameBehaviour.GameTimeOffset;
        if(_setTargetAtSpawn)
            tether.SetTarget(_playerObject.transform);
    }
}
