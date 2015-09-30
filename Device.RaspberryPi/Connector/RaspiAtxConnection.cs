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
    public class RaspiAtxConnection : IDisposable
    {
        private static readonly ILog log = LogManager.GetLogger<RaspiAtxConnection>();
        private readonly GpioConnection _gpioConnection;
        private OutputPinConfiguration _bookOkPin;

        public RaspiAtxConnection(IController controller)
        {
            // set boot ok pin
            _bookOkPin = ConnectorPin.P1Pin11.Output().Revert();

            // listen to shutdown gpio
            var shutdownPin = ConnectorPin.P1Pin13
                .Input()
                .PullDown()
                .OnStatusChanged(state =>
                {
                    if (state)
                        controller.Shutdown();
                });

            // open connection
            _gpioConnection = new GpioConnection(_bookOkPin, shutdownPin);
        }

        public void SetBookOk()
        {
            log.Debug("Set RaspiAtx boot ready to high");
            _bookOkPin.Enable();
        }

        public void Dispose()
        {
            _gpioConnection?.Close();
        }
    }
}
