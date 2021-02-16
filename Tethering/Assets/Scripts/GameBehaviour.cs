using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameBehaviour : MonoBehaviour
{
    [SerializeField]
    private float _gameRestartDelay = 1.0f;

    public void Restart()
    {
        PointBehaviour.Score = 0;
        StartCoroutine(LoadSceneAfterDelay());
    }

    IEnumerator LoadSceneAfterDelay()
    {
        yield return new WaitForSeconds(_gameRestartDelay);
        SceneManager.LoadScene(0);
    }
}
