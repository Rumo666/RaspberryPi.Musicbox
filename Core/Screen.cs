using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jukebox.Core
{
    public abstract class Screen
    {
        public abstract DisplayContent Render(VirtualDisplay display);
    }
}
