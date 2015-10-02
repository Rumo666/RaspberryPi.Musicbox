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

        public TimeSpan Speed { get; set; }

        public string Render(string content, IDisplay display)
        {
            // need to scroll
            if (content.Length <= display.Columns)
                return content;

            return null;
        }
    }
}
