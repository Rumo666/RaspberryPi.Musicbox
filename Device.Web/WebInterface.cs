using System;
using Common.Logging;
using Jukebox.Core;
using Microsoft.Owin.Hosting;

namespace Jukebox.Device.Web
{
    public class WebInterface : IDevice, IDisposable
    {
        private static readonly ILog log = LogManager.GetLogger<WebInterface>();
        private const string hostUrl = "http://*:9123/";
        private readonly IDisposable _server;
        
        public static string LcdLine1 { get; private set; }
        public static string LcdLine2 { get; private set; }

        #region Constructor

        public WebInterface(IController controller)
        {
            log.Debug(m => m("Startup webserver at '{0}'", hostUrl));

            Controller = controller;
            _server = WebApp.Start<Startup>(hostUrl);
        }

        #endregion

        #region Properties

        public static IController Controller { get; private set; }

        #endregion

        #region IDevice

        public void ShowScreen(IScreen screen, TimeSpan? timeout)
        {
            // todo update to virtual screen system
            //LcdLine1 = line1;
            //LcdLine2 = line2;
        }

        public void ProcessCycle()
        {
            // nothing to do
        }

        public void SetVolume(byte value)
        {
            // nothing to do
        }

        public void Initalize()
        {
            // nothing to do
        }

        public void Shutdown()
        {
            // nothing to do
        }

        public void InitShutdown()
        {
            // nothing to do
        }

        #endregion

        #region IDisposable

        public void Dispose()
        {
            if (_server != null)
                _server.Dispose();
        }

        #endregion

    }
}
