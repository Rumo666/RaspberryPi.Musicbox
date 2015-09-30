using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jukebox.Core;

namespace Jukebox.Device.Web.Model
{
    public class DeviceState
    {
        public PlayerStatus Player { get; set; }
        public string LcdLine1 { get; set; }
        public string LcdLine2 { get; set; }
    }
}
