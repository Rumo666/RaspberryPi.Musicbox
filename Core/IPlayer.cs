using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jukebox.Core
{
    public interface IPlayer
    {
        void PlayNext();
        void PlayPrevious();
        void PlayFirst();
        void Stop();
        void Play();
        void Play(string id);
        void Pause(bool pause);
        void SetVolume(byte volume);
        PlayerStatus GetStatus();
    }
}
