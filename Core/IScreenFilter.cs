using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jukebox.Core
{
    public interface IScreenFilter
    {
        string Render(string content, IDisplay display);
    }
}
