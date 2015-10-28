using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jukebox.Core.Renderer;

namespace Jukebox.Core.Screens
{
    public class MainScreen : IScreen
    {
        private readonly ScrollRenderer _scrollRenderer;
        private readonly ScreenContent _content = new ScreenContent();

        public MainScreen()
        {
            _scrollRenderer = new ScrollRenderer(ScrollRenderer.Modus.Infinite, new TimeSpan(0, 0, 0, 0, 150), new TimeSpan(0, 0, 0, 3));
        }

        public PlayerStatus PlayerStatus { get; set; }

        public ScreenContent Render(IDisplay display)
        {
            var state = (PlayerStatus.State == PlayerStatus.States.Play
                ? ScreenContent.LcdCharacterPlay
                : PlayerStatus.State == PlayerStatus.States.Pause
                    ? ScreenContent.LcdCharacterPause
                    : ScreenContent.LcdCharacterStop);

            _content.Clear();
            _content.AppendRow($"{(char)state} {PlayerStatus.TrackNumber}/{PlayerStatus.PlaylistLength} {(int)PlayerStatus.CurrentPosition.TotalMinutes:00}:{PlayerStatus.CurrentPosition.Seconds:00}");
            _content.AppendRow(_scrollRenderer.Render($"{PlayerStatus.Title}", display));

            return _content;
        }

        public void Reset()
        {
            _scrollRenderer.Reset();
        }
    }
}
