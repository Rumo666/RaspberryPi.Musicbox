using Jukebox.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Common.Logging;
using Jukebox.Device.Web;
using Jukebox.Player.Dummy;
using Jukebox.Device.RaspberryPi;
using Jukebox.Player.Mpc;
using System.Net;

namespace Jukebox.Runtime
{
    class Program
    {
        private static readonly ILog log = LogManager.GetLogger<Program>();

        static void Main(string[] args)
        {
            IDevice[] devices = null;

            try
            {
                log.Info(m => m("Application start"));

                //var player = new DummyPlayer();
                var player = new MpcPlayer(new IPEndPoint(IPAddress.Loopback, 6600));
                var controller = new Controller(player);
                var pi = new RaspberryPi(controller);
                //var web = new WebInterface(contoller);
                devices = new IDevice[] { pi };

                // init controller
                controller.Initalize(devices);

                // process loop
                log.Debug(m => m("Start process loop"));

                while (!Console.KeyAvailable)
                {
                    // process
                    controller.ProcessCycle();

                    // exit if shutdown is triggered
                    if (controller.IsShuttingDown)
                    {
                        break;
                    }

                    // throttle execution
                    Thread.Sleep(100);
                }
            }
            catch (Exception ex)
            {
                log.Error("Error", ex);
            }
            finally
            {
                log.Info(m => m("Application terminate"));

                if (devices != null)
                {
                    foreach (var device in devices)
                    {
                        device.Dispose();
                    }
                }
            }
        }
    }
}
