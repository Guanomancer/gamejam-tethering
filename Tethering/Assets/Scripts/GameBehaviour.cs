using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameBehaviour : MonoBehaviour
{
    public void Restart()
    {
        SceneManager.LoadScene(0);
    }
}
