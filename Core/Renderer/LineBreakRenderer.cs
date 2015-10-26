using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jukebox.Core.Renderer
{
    public class LineBreakRenderer
    {
        public ScreenContent Render(string content, IDisplay display)
        {
            var parts = content.Split(' ');
            var output = new ScreenContent();

            return new ScreenContent();
        }
    }
}
