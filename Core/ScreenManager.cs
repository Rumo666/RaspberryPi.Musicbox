using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jukebox.Core
{
    public class ScreenManager
    {
        private IScreen _stackedScreen;
        private IScreen _currentScreen;
        private DateTime _stackedScreenEnd;

        public void SetScreen(IScreen screen, TimeSpan? timeout = null)
        {
            if (timeout == null)
            {
                _currentScreen = screen;
                //_stackedScreen = null;
            }
            else
            {
                _stackedScreen = screen;
                _stackedScreenEnd = DateTime.Now.Add(timeout.Value);
            }
        }

        public IScreen GetCurrentScreen()
        {
            return (_stackedScreen != null && _stackedScreenEnd >= DateTime.Now
                ? _stackedScreen
                : _currentScreen);
        }
    }
}
