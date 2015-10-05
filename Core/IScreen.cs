using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jukebox.Core
{
    public interface IScreen
    {
        ScreenContent Render(IDisplay display);
    }
}
