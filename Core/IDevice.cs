using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jukebox.Core
{
    public interface IDevice
    {
        void Process();
        void DisplayText(string line1, string line2, TimeSpan? timeout);
        void SetVolume(byte value);
        void Initalize();
        void Shutdown();
    }
}
