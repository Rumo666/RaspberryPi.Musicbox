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
        private int _scrollDirection;

        public ScreenFilterScroll(TimeSpan speed, TimeSpan? pause = null)
        {
            Speed = speed;
            Pause = pause ?? new TimeSpan(0);

            Reset();
        }

        public TimeSpan Speed { get; set; }

        public TimeSpan Pause { get; set; }

        public string Render(string content, IDisplay display)
        {
            // need to scroll
            if (content.Length <= display.Columns)
            {
                // reset
                _position = 0;
                _scrollDirection = 0;

                return content;
            }

            // update scroll position
            if (_scrollDirection != 0 && _lastPositionChange.Add(Speed) < DateTime.Now)
            {
                _position = _position + _scrollDirection;

                // pause at end
                if (_position < 0)
                {
                    _position = 0;
                    _scrollDirection = 0;
                }
                else if (_position > content.Length - display.Columns)
                {
                    _position = content.Length - display.Columns;
                    _scrollDirection = 0;
                }

                _lastPositionChange = DateTime.Now;
            }

            // reverse direction after pause
            if (_scrollDirection == 0 && _lastPositionChange.Add(Pause) < DateTime.Now)
            {
                _scrollDirection = (_position == 0 ? 1 : -1);
            }

            return content.Substring(_position, display.Columns);
        }

        public void Reset()
        {
            _position = 0;
            _scrollDirection = 0;
            _lastPositionChange = DateTime.Now;
        }
    }
}
