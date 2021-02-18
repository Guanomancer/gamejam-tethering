using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tethering.Net
{
    public class GameInfo
    {
        private const int GAME_TIMEOUT_SECONDS = 4000;

        public readonly string Key;
        public readonly DateTime UtcStartTime = DateTime.UtcNow;
        public readonly int RemoteAddress;

        public long SecondsSinceStart { get => (long)DateTime.UtcNow.Subtract(UtcStartTime).TotalSeconds; }
        public bool HasTimedOut { get => SecondsSinceStart > GAME_TIMEOUT_SECONDS; }

        public GameInfo (string key, int remoteAddress)
        {
            Key = key;
            RemoteAddress = remoteAddress;
        }

        public bool ValidateScore(int score)
        {
            return true;
        }

        public bool ValidateName(string name)
        {
            return true;
        }
    }
}
