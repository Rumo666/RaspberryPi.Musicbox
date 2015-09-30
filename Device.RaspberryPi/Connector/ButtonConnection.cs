using Common.Logging;
using Jukebox.Core;
using Raspberry.IO.GeneralPurpose;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jukebox.Device.RaspberryPi.Connector
{
    public class ButtonConnection : IDisposable
    {
        private static readonly ILog log = LogManager.GetLogger<ButtonConnection>();

        private readonly GpioConnection _gpioConnection;

        public ButtonConnection(IController contoller)
        {
            log.Debug(m => m("Init button connection"));

            var toggleBtn = ConnectorPin.P1Pin12.Input().PullDown();
            var backBtn = ConnectorPin.P1Pin16.Input().PullDown();
            var nextBtn = ConnectorPin.P1Pin18.Input().PullDown();

            // toggle
            toggleBtn.OnStatusChanged(state => 
            { 
                if (state)
                    contoller.TogglePlay(); 
            });

            // next
            nextBtn.OnStatusChanged(state =>
            {
                if (state)
                    contoller.PlayNext();
            });

            // previous
            backBtn.OnStatusChanged(state =>
            {
                if (state)
                    contoller.PlayPrevious();
            });

            // open connection
            _gpioConnection = new GpioConnection(toggleBtn, backBtn, nextBtn);
        }

        public void Dispose()
        {
            _gpioConnection?.Close();
        }
    }
}
