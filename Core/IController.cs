using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jukebox.Core
{
    public interface IController
    {
        void TogglePlay();
        void PlayNext();
        void PlayPrevious();
        void SetVolume(byte value);
        void Shutdown();
        PlayerStatus GetPlayerState();
        void PlayByTagId(string id);
        bool IsShuttingDown { get; }
        void ProcessCycle();
    }
}
