using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using TMPro;
using UnityEngine.Networking;
using UnityEngine;

namespace Tethering.Net
{
    public class HttpClient
    {
        public readonly string HostnameOrAddress;
        public readonly int Port;

        public bool UseUnity = false;

        public HttpClient(string hostnameOrAddress, int port)
        {
            HostnameOrAddress = hostnameOrAddress;
            Port = port;
        }

        public IEnumerator StartGame(MonoBehaviour behaviour, Action<bool, int, string> onCompleted)
        {
            yield return SendRequestAndProcessResponse(behaviour, "START_GAME", null, onCompleted);
        }

        public IEnumerator EndGame(MonoBehaviour behaviour, string gameKey, int points, string playerName, Action<bool, int, string> onCompleted)
        {
            yield return SendRequestAndProcessResponse(behaviour, "END_GAME", $"{gameKey}\n{points}\n{playerName}", onCompleted);
        }

        public IEnumerator GetDailyScoreBoard(MonoBehaviour behaviour, Action<bool, int, string> onCompleted)
        {
            yield return SendRequestAndProcessResponse(behaviour, "GET_DAILY", null, onCompleted);

            //if (SendRequestAndGetResponse("GET_DAILY", null, out int code, out string text))
            //{
            //    GetScoreBoard(text, out entries);
            //    return true;
            //}
            //else
            //{
            //    entries = null;
            //    return false;
            //}
        }

        public IEnumerator GetInfiniteScoreBoard(MonoBehaviour behaviour, Action<bool, int, string> onCompleted)
        {
            yield return SendRequestAndProcessResponse(behaviour, "GET_INFINITE", null, onCompleted);

            //if (SendRequestAndGetResponse("GET_INFINITE", null, out int code, out string text))
            //{
            //    GetScoreBoard(text, out entries);
            //    return true;
            //}
            //else
            //{
            //    entries = null;
            //    return false;
            //}
        }

        public void GetScoreBoard(string text, out List<ScoreEntry> entries)
        {
            var entryStrings = text.Split('\n');
            entries = new List<ScoreEntry>();
            foreach (var str in entryStrings)
            {
                var items = str.Split('\r');
                if (items.Length == 2)
                    entries.Add(new ScoreEntry(int.Parse(items[0]), items[1]));
            }
        }

        public IEnumerator SendRequestAndProcessResponse(MonoBehaviour behaviour, string method, string content, Action<bool, int, string> onCompleted)
        {
            UnityWebRequest request;
            if (content == null)
                request = UnityWebRequest.Get($"http://{HostnameOrAddress}:{Port}");
            else
            {
                request = UnityWebRequest.Post($"http://{HostnameOrAddress}:{Port}", content);
                request.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(content));
            }
            request.method = method;
            yield return request.SendWebRequest();

            bool success;
            int code;
            string text;

            if (request.result == UnityWebRequest.Result.Success)
            {
                using (var stream = new MemoryStream(request.downloadHandler.data))
                {
                    byte[] buffer = new byte[10240];
                    int bytesReceived = stream.Read(buffer, 0, buffer.Length);
                    Console.WriteLine($"Received {bytesReceived} bytes from the remote host.");

                    success = true;
                    code = 200;
                    text = Encoding.UTF8.GetString(buffer, 0, bytesReceived);
                }
            }
            else
            {
                success = false;
                code = 404;
                text = null;
            }
            onCompleted(success, code, text);
        }
    }
}
