using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jukebox.Core.Screens
{
    public class MainScreen : Screen
    {
        public PlayerStatus PlayerStatus { get; set; }
        public override DisplayContent Render(VirtualDisplay display)
        {
            throw new NotImplementedException();
        }
    }
}
