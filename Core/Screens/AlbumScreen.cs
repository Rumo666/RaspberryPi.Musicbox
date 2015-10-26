using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jukebox.Core.Screens
{
    public class AlbumScreen : IScreen
    {
        private readonly PlayerStatus _playerStatus;

        public AlbumScreen(PlayerStatus playerStatus)
        {
            _playerStatus = playerStatus;
        }

        public ScreenContent Render(IDisplay display)
        {
            return null;
        }
    }
}
