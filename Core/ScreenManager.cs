using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jukebox.Core
{
    public class ScreenManager
    {
        private Screen _stackedScreen;
        private Screen _currentScreen;
        private DateTime _stackedScreenEnd;

        public void SetScreen(Screen screen, TimeSpan? timeout = null)
        {
            if (timeout == null)
            {
                _currentScreen = screen;
            }
            else
            {
                _stackedScreen = screen;
                _stackedScreenEnd = DateTime.Now.Add(timeout.Value);
            }
        }

        public DisplayContent Render()
        {
            var screen = (_stackedScreen != null && _stackedScreenEnd >= DateTime.Now
                ? _stackedScreen
                : _currentScreen);

            return screen.Render(this);
        }
    }
}
