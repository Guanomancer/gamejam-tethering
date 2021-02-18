using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Tethering.Net;
using TMPro;
using UnityEngine;

public class TetherNetBehaviour : MonoBehaviour
{
    [SerializeField]
    private string _hostnameOrIpAddress = "tethig.guanomancer.com";
    [SerializeField]
    private int _port = 42080;

    [SerializeField]
    private TextMeshProUGUI _connectionErrorTextField;

    public string CurrentGameKey;
    public List<ScoreEntry> DailyBoard;
    public List<ScoreEntry> AllTimeBoard;

    private HttpClient _client;

    public HttpClient Client { get => _client; }

    private void Awake()
    {
        _client = new HttpClient(_hostnameOrIpAddress, _port);
        _client.UseUnity = true;
    }

    public bool IsScoreHigh(int score)
    {
        return DailyBoard != null && AllTimeBoard != null &&
            score > 0 && (
            (DailyBoard.Count < 10 || score > DailyBoard[9].Points)
            ||
            (AllTimeBoard.Count < 10 || score > AllTimeBoard[9].Points)
            );
    }

    public void StartGame()
    {
        StartGameUnthread();
        //StartCoroutine(StartGameThread());
    }

    private IEnumerator StartGameThread()
    {
        bool success = false;
        var thread = new Thread(new ThreadStart(() =>
        {
            Client.GetInfiniteScoreBoard(out AllTimeBoard);
            Client.GetInfiniteScoreBoard(out DailyBoard);
            success = _client.StartGame(out CurrentGameKey);
        }));
        thread.Start();
        while (thread.IsAlive)
            yield return new WaitForSeconds(.1f);

        _connectionErrorTextField.gameObject.SetActive(!success);
    }

    private void StartGameUnthread()
    {
        bool success = false;
        Client.GetInfiniteScoreBoard(out AllTimeBoard);
        Client.GetInfiniteScoreBoard(out DailyBoard);
        success = _client.StartGame(out CurrentGameKey);
        _connectionErrorTextField.gameObject.SetActive(!success);
    }
}
