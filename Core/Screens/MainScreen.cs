using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jukebox.Core.Screens
{
    public class MainScreen : IScreen
    {
        private readonly ScreenFilterScroll _titleScrollFilter;

        public MainScreen()
        {
            _titleScrollFilter = new ScreenFilterScroll(new TimeSpan(0, 0, 0, 0, 150), new TimeSpan(0, 0, 0, 3));
        }

        public PlayerStatus PlayerStatus { get; set; }

        public ScreenContent Render(IDisplay display)
        {
            var state = (PlayerStatus.State == PlayerStatus.States.Play
                ? GenericScreen.LcdCharacterPlay
                : PlayerStatus.State == PlayerStatus.States.Pause
                    ? GenericScreen.LcdCharacterPause
                    : GenericScreen.LcdCharacterStop);


            var line1 = $"{(char)state} {PlayerStatus.TrackNumber}/{PlayerStatus.PlaylistLength} {(int)PlayerStatus.CurrentPosition.TotalMinutes:00}:{PlayerStatus.CurrentPosition.Seconds:00}";
            var line2 = $"{PlayerStatus.Title}";

            return new ScreenContent
            {
                Line1 = line1,
                Line2 = _titleScrollFilter.Render(line2, display)
            };
        }

        public void Reset()
        {
            _titleScrollFilter.Reset();
        }
    }
}
