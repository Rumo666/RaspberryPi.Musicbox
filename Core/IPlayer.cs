using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jukebox.Core
{
    public interface IPlayer
    {
        void Next();
        void Previous();
        void Stop();
        void Play();
        void Play(string id);
        void Pause(bool pause);
        void SetVolume(byte volume);
        PlayerStatus GetStatus();
    }
}
