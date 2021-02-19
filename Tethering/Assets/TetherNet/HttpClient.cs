using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using UnityEngine.Networking;

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

        public bool StartGame(out string gameKey)
        {
            if (SendRequestAndGetResponse("START_GAME", null, out int code, out string text))
            {
                gameKey = text;
                return true;
            }
            else
            {
                gameKey = null;
                return false;
            }
        }

        public bool EndGame(string gameKey, int points, string playerName)
        {
            if (SendRequestAndGetResponse("END_GAME", $"{gameKey}\n{points}\n{playerName}", out int code, out string text))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool GetDailyScoreBoard(out List<ScoreEntry> entries)
        {
            if (SendRequestAndGetResponse("GET_DAILY", null, out int code, out string text))
            {
                GetScoreBoard(text, out entries);
                return true;
            }
            else
            {
                entries = null;
                return false;
            }
        }

        public bool GetInfiniteScoreBoard(out List<ScoreEntry> entries)
        {
            if (SendRequestAndGetResponse("GET_INFINITE", null, out int code, out string text))
            {
                GetScoreBoard(text, out entries);
                return true;
            }
            else
            {
                entries = null;
                return false;
            }
        }

        private void GetScoreBoard(string text, out List<ScoreEntry> entries)
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

        public bool SendGet()
        {
            if (SendRequestAndGetResponse("GET", null, out int code, out string text))
            {
                return true;
            }
            else
                return false;
        }

        private bool SendRequestAndGetResponse(string method, string content, out int code, out string text)
        {
            if (!UseUnity)
            {

                var request = HttpWebRequest.Create($"http://{HostnameOrAddress}:{Port}");
                request.Timeout = 5000;
                request.Method = method;
                if (content != null)
                {
                    using (var stream = request.GetRequestStream())
                    {
                        var buffer = Encoding.UTF8.GetBytes(content);
                        stream.Write(buffer, 0, buffer.Length);
                        stream.Close();
                    }
                }
                try
                {
                    using (var response = request.GetResponse())
                    {
                        using (var stream = response.GetResponseStream())
                        {
                            byte[] buffer = new byte[10240];
                            int bytesReceived = stream.Read(buffer, 0, buffer.Length);
                            Console.WriteLine($"Received {bytesReceived} bytes from the remote host.");
                            code = 200;
                            text = Encoding.UTF8.GetString(buffer, 0, bytesReceived);
                            return true;
                        }
                    }
                }
                catch
                {
                    code = 404;
                    text = null;
                    return false;
                }
            }
            else
            {
                var request = UnityWebRequest.Get($"http://{HostnameOrAddress}:{Port}");
                request.method = method;
                request.SendWebRequest();
                if (request.result == UnityWebRequest.Result.Success)
                {
                    while (!request.isDone) ; // must run as coroutine
                    using (var stream = new MemoryStream(request.downloadHandler.data))
                    {
                        byte[] buffer = new byte[10240];
                        int bytesReceived = stream.Read(buffer, 0, buffer.Length);
                        Console.WriteLine($"Received {bytesReceived} bytes from the remote host.");
                        code = 200;
                        text = Encoding.UTF8.GetString(buffer, 0, bytesReceived);
                        return true;
                    }
                }
                else
                {
                    code = 404;
                    text = null;
                    return false;
                }
            }
        }

        private IEnumerator SendRequestAndProcessResponse(string method, string content, Action<bool, int, string> onCompleted)
        {
            UnityWebRequest request;
            if(content == null)
                request = UnityWebRequest.Get($"http://{HostnameOrAddress}:{Port}");
            else
                request = UnityWebRequest.Post($"http://{HostnameOrAddress}:{Port}", content);
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
