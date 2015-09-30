using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jukebox.Core
{
    public class PlayerStatus
    {
        public enum States
        {
            Play,
            Stop,
            Pause
        }

        public States State { get; set; }
        public string Title { get; set; }
        public string Album { get; set; }
        public string Artist { get; set; }
        public string TagId { get; set; }
        public byte Volume { get; set; }
        public int PlaylistLength { get; set; }
        public int TrackNumber { get; set; }
        public TimeSpan CurrentPosition { get; set; }
    }
}
