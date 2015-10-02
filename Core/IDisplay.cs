using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jukebox.Core
{
    public interface IDisplay
    {
        int Columns { get; }
        int Rows { get; }
    }
}
