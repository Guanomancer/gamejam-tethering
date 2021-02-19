using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
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
    private TetherNetBehaviour _tetherNet;

    [SerializeField]
    private GameObject _highscoreText;

    [SerializeField]
    private TextMeshProUGUI _highscorePlayerName;

    [SerializeField]
    private GameObject _connectionErrorTextField;

    [SerializeField]
    private HighScoreLoaderBehaviour _dailyHighScore;
    [SerializeField]
    private HighScoreLoaderBehaviour _allTimeHighScore;

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
        _tetherNet.StartGame();
    }

    public void EndGame()
    {
        if (_tetherNet.IsScoreHigh(PointBehaviour.Score))
            _highscoreText.SetActive(true);
    }

    public void RegisterHighscore()
    {
        StartCoroutine(_tetherNet.Client.SendRequestAndProcessResponse(
               "END_GAME", $"{_tetherNet.CurrentGameKey}\n{PointBehaviour.Score}\n{_highscorePlayerName.text}", (success, code, response) =>
               {
                   _connectionErrorTextField.gameObject.SetActive(!success);
                   _dailyHighScore.Reload();
                   _allTimeHighScore.Reload();
               }
               ));
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
