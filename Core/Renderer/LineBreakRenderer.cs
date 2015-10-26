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
            //var parts = content.Split(' ');
            //var output = new ScreenContent();
            //var pos = 0;

            //foreach (var part in parts)
            //{
            //    // fit part on current line
            //    if (part.Length + pos < display.Columns)
            //    {
            //        output.AppendContent(part + " ");
            //        pos += part.Length + 1;

            //        continue;
            //    }

            //    // fit part on a whole line
            //    if 
            //}

            var output = new ScreenContent();
            var pos = 0;

            for (int row = 0; row < display.Rows; row++)
            {
                // stop if content is too short
                if (pos >= content.Length)
                    break;

                // skip empty space
                if (content[pos] == ' ')
                    pos++;

                var end = pos + display.Columns;

                output.AppendRow(
                    content.Substring(
                        pos, 
                        (end > content.Length 
                            ? content.Length - pos
                            : end - pos)));

                pos += display.Columns;
            }

            return output;
        }
    }
}
