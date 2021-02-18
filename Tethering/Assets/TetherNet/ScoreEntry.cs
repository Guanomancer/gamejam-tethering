using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tethering.Net
{
    public class ScoreEntry
    {
        public int Points;
        public string Player;
        public DateTime UtcTime = DateTime.UtcNow;
        public string RemoteEndPoint;

        public ScoreEntry(int points, string player, string remoteEndPoint = null)
        {
            Points = points;
            Player = player;
            RemoteEndPoint = remoteEndPoint;
        }
    }
}
