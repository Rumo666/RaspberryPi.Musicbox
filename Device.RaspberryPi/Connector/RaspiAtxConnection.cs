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
        private GpioOutputBinaryPin _triggerOnOffButtonPin;

        public RaspiAtxConnection(IController controller, IGpioConnectionDriver driver)
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
                    {
                        log.Debug("Shutdown triggered by RaspiAtx");

                        controller.Shutdown();
                    }
                });

            // connect on/off button trigger pin
            _triggerOnOffButtonPin = driver.Out(ConnectorPin.P1Pin32);

            // open connection
            _gpioConnection = new GpioConnection(_bookOkPin, shutdownPin);
        }

        public void SetBookOk()
        {
            log.Debug("Set RaspiAtx boot ready to high");

            _bookOkPin.Enable();
        }

        public void SetOnOffButtonState(bool high)
        {
            log.Debug(m => m("Set RaspiAtx on/off button {0}", high));

            _triggerOnOffButtonPin.Write(high);
        }

        public void Dispose()
        {
            _gpioConnection?.Close();
        }
    }
}
