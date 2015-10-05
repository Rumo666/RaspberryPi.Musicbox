using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jukebox.Core.Screens
{
    public class GenericScreen : IScreen
    {
        public const byte LcdCharacterPlay = 0x0;
        public const byte LcdCharacterPause = 0x1;
        public const byte LcdCharacterStop = 0x2;

        private string _line1;
        private string _line2;

        public GenericScreen(string line1, string line2)
        {
            _line1 = line1;
            _line2 = line2;
        }

        public ScreenContent Render(IDisplay display)
        {
            return new ScreenContent
            {
                Line1 = _line1,
                Line2 = _line2
            };
        }
    }
}
