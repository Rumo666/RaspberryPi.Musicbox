using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jukebox.Core
{
    public interface IDevice : IDisposable
    {
        void ProcessCycle();
        void ShowScreen(IScreen screen, TimeSpan? timeout);
        void SetVolume(byte value);
        void Initalize();
        void Shutdown();
    }
}
