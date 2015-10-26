using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jukebox.Core.Renderer;

namespace Jukebox.Core.Screens
{
    public class AlbumScreen : IScreen
    {
        private readonly PlayerStatus _playerStatus;
        private readonly LineBreakRenderer _lineBreakRenderer = new LineBreakRenderer();

        public AlbumScreen(PlayerStatus playerStatus)
        {
            _playerStatus = playerStatus;
        }

        public ScreenContent Render(IDisplay display)
        {
            return _lineBreakRenderer.Render(_playerStatus.Album, display);
        }
    }
}
