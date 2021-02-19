using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class GameBehaviour : MonoBehaviour
{
    [SerializeField]
    private float _gameRestartDelay = 1.0f;

    [SerializeField]
    private GameObject _evilTetherPrefab;

    [SerializeField]
    private GameObject _playerObject;

    [HideInInspector]
    public float GameTimeOffset = 0;

    [SerializeField]
    private GameObject _highscoreText;

    [SerializeField]
    private TextMeshProUGUI _highscorePlayerName;

    [SerializeField]
    private GameObject _connectionErrorTextField;

    private void Awake()
    {
        PointBehaviour.Score = 0;
    }

    public void StartGame()
    {
        GameTimeOffset = Time.time;
        var evilSpawn = Random.insideUnitSphere * 20f;
        evilSpawn.y = 0;
        evilSpawn.x += 10f;
        evilSpawn.z += 10f;
        var evil = Instantiate(_evilTetherPrefab, evilSpawn, Quaternion.identity);
        evil.GetComponent<TetherControlBehaviour>().SetTarget(_playerObject.transform);
    }

    public void Restart()
    {
        StartCoroutine(LoadSceneAfterDelay());
    }

    IEnumerator LoadSceneAfterDelay()
    {
        yield return new WaitForSeconds(_gameRestartDelay);
        SceneManager.LoadScene(0);
    }
}
