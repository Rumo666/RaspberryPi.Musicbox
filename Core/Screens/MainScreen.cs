using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jukebox.Core.Screens
{
    public class MainScreen : Screen
    {
        public PlayerStatus PlayerStatus { get; set; }

        public override DisplayContent Render(ScreenManager display)
        {
            var state = (PlayerStatus.State == PlayerStatus.States.Play
                ? LcdCharacterPlay
                : PlayerStatus.State == PlayerStatus.States.Pause
                    ? LcdCharacterPause
                    : LcdCharacterStop);


            var line1 = $"{(char)state} {PlayerStatus.TrackNumber}/{PlayerStatus.PlaylistLength} {(int)PlayerStatus.CurrentPosition.TotalMinutes:00}:{PlayerStatus.CurrentPosition.Seconds:00}";
            var line2 = $"{PlayerStatus.Title}";

            return new DisplayContent
            {
                Line1 = line1,
                Line2 = line2
            };
        }
    }
}
