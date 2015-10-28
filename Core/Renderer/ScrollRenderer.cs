using System;

namespace Jukebox.Core.Renderer
{
    public class ScrollRenderer
    {
        public enum Modus
        {
            Bounce,
            Infinite
        }

        private int _position;
        private DateTime _lastPositionChange;
        private int _scrollDirection;

        public ScrollRenderer(Modus modus, TimeSpan speed, TimeSpan? pause = null)
        {
            Speed = speed;
            Pause = pause ?? new TimeSpan(0);
            Mode = modus;

            Reset();
        }

        public TimeSpan Speed { get; set; }

        public TimeSpan Pause { get; set; }

        public Modus Mode { get; set; }

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

            switch (Mode)
            {
                case Modus.Bounce:
                    return RenderBounce(content, display);

                case Modus.Infinite:
                    return RenderInfinite(content, display);

                default:
                    throw new Exception($"Scroll mode '{Mode}' is not implemented");
            }
        }

        private string RenderInfinite(string content, IDisplay display)
        {
            // append " - "
            content += " - ";

            // update scroll position
            if (_scrollDirection != 0 && _lastPositionChange.Add(Speed) < DateTime.Now)
            {
                _position++;

                // reset position at end and pause
                if (_position >= content.Length)
                {
                    _position = 0;
                    _scrollDirection = 0;
                }

                _lastPositionChange = DateTime.Now;
            }

            // start again after pause
            if (_scrollDirection == 0 && _lastPositionChange.Add(Pause) < DateTime.Now)
            {
                _scrollDirection = 1;
            }

            var partLength = (content.Length - _position > display.Columns 
                ? display.Columns 
                : content.Length - _position);

            return content.Substring(_position, partLength) + content.Substring(0, display.Columns - partLength);
        }

        private string RenderBounce(string content, IDisplay display)
        {
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
