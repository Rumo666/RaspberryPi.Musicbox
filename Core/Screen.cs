using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jukebox.Core
{
    public abstract class Screen
    {
        public const byte LcdCharacterPlay = 0x0;
        public const byte LcdCharacterPause = 0x1;
        public const byte LcdCharacterStop = 0x2;

        public abstract ScreenContent Render(ScreenManager display);
    }
}
