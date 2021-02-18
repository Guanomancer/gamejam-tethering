using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Tethering.Net;
using TMPro;

public class HighScoreLoaderBehaviour : MonoBehaviour
{
    public enum BoardType { Daily, AllTime }

    [SerializeField]
    private string _failedToLoadText = "Failed to load high score. Server may be down.";

    [SerializeField]
    private TetherNetBehaviour _tetherNet;

    [SerializeField]
    private GameObject _highScoreEntryPrefab;

    [SerializeField]
    private BoardType _boardType;

    [SerializeField]
    private TextMeshProUGUI _title;

    private string _currentGameID;

    private List<ScoreEntry> _board;

    private List<GameObject> _entries = new List<GameObject>();

    public void Reload()
    {
        foreach (var entry in _entries)
            Destroy(entry);
        _entries.Clear();

        LoadUnthread();
        //StartCoroutine(LoadThread());
    }

    private void OnEnable() => Reload();

    private IEnumerator LoadThread()
    {
        bool wasLoadet = false;
        var thread = new Thread(new ThreadStart(() =>
        {
            switch (_boardType)
            {
                case BoardType.Daily:
                    wasLoadet = _tetherNet.Client.GetDailyScoreBoard(out _board);
                    break;
                case BoardType.AllTime:
                default:
                    wasLoadet = _tetherNet.Client.GetInfiniteScoreBoard(out _board);
                    break;
            }
        }));
        thread.Start();
        while (thread.IsAlive)
            yield return new WaitForSeconds(.1f);
        if (wasLoadet)
        {
            foreach (var entry in _board)
            {
                var obj = Instantiate(_highScoreEntryPrefab, transform);
                _entries.Add(obj);
                var fields = obj.GetComponentsInChildren<TextMeshProUGUI>();
                fields[0].text = entry.Player;
                fields[1].text = entry.Points.ToString();
            }
        }
        else
            _title.text = _failedToLoadText;
    }

    private void LoadUnthread()
    {
        bool wasLoadet = false;
        switch (_boardType)
        {
            case BoardType.Daily:
                wasLoadet = _tetherNet.Client.GetDailyScoreBoard(out _board);
                break;
            case BoardType.AllTime:
            default:
                wasLoadet = _tetherNet.Client.GetInfiniteScoreBoard(out _board);
                break;
        }
        if (wasLoadet)
        {
            foreach (var entry in _board)
            {
                var obj = Instantiate(_highScoreEntryPrefab, transform);
                _entries.Add(obj);
                var fields = obj.GetComponentsInChildren<TextMeshProUGUI>();
                fields[0].text = entry.Player;
                fields[1].text = entry.Points.ToString();
            }
        }
        else
            _title.text = _failedToLoadText;
    }
}
