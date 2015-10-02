using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jukebox.Core
{
    public class ScreenFilterScroll : IScreenFilter
    {
        private int _position;
        private DateTime _lastPositionChange;
        private bool _scrollBackward;

        public ScreenFilterScroll(TimeSpan speed)
        {
            Speed = speed;
        }

        public TimeSpan Speed { get; set; }

        public string Render(string content, IDisplay display)
        {
            // need to scroll
            if (content.Length <= display.Columns)
            {
                // reset
                _position = 0;
                _scrollBackward = false;

                return content;
            }

            // update position
            if (_lastPositionChange.Add(Speed) < DateTime.Now)
            {
                _position = _position + (_scrollBackward ? -1 : 1);

                // revert position
                if (_position < 0)
                {
                    _position = 0;
                    _scrollBackward = false;
                }
                else if (_position > content.Length - display.Columns)
                {
                    _position = content.Length - display.Columns;
                    _scrollBackward = true;
                }

                _lastPositionChange = DateTime.Now;
            }

            return content.Substring(_position, display.Columns);
        }
    }
}
