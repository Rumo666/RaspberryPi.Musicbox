using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jukebox.Core.Screens
{
    public class GenericScreen : Screen
    {
        private string _line1;
        private string _line2;

        public GenericScreen(string line1, string line2)
        {
            _line1 = line1;
            _line2 = line2;
        }

        public override ScreenContent Render(ScreenManager display)
        {
            return new ScreenContent
            {
                Line1 = _line1,
                Line2 = _line2
            };
        }
    }
}
