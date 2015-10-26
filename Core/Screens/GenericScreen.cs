using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jukebox.Core.Screens
{
    public class GenericScreen : IScreen
    {
        private readonly ScreenContent _content;

        public GenericScreen(IEnumerable<string> rows)
        {
            _content = new ScreenContent(rows);
        }

        public GenericScreen(ScreenContent content)
        {
            _content = content;
        }

        public ScreenContent Render(IDisplay display)
        {
            return _content;
        }
    }
}
