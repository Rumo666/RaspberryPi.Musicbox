using Common.Logging;
using Raspberry.IO.GeneralPurpose;
using Raspberry.IO.InterIntegratedCircuit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jukebox.Device.RaspberryPi.Connector
{
    public class AmplifierConnection : IDisposable
    {
        private static readonly ILog log = LogManager.GetLogger<AmplifierConnection>();
        private readonly I2cDeviceConnection _connection;
        private readonly I2cDriver _i2cDriver;
        private readonly OutputPinConfiguration _shutdownPin;
        private readonly GpioConnection _gpioConnection;

        public AmplifierConnection()
        {
            log.Debug(m => m("Init amplifier connection"));

            // connect volume via i2c
            _i2cDriver = new I2cDriver(ProcessorPin.Pin2, ProcessorPin.Pin3);
            _connection = _i2cDriver.Connect(0x4b);

            // connect shutdown via gpio
            _shutdownPin = ConnectorPin.P1Pin36.Output().Revert();
            _gpioConnection = new GpioConnection(_shutdownPin);
        }

        public void SetVolume(byte value)
        {
            var data = (byte)(value / 255f * 63);

            //log.Debug(m => m("Set amplifier volume to {0}", data));

            _connection.WriteByte(data);
        }

        public void SetShutdownState(bool enabled)
        {
            log.Debug(m => m("Set amplifier shutdown state to {0}", enabled));

            if (enabled)
                _shutdownPin.Disable();
            else
                _shutdownPin.Enable();
        }

        public void Dispose()
        {
            _i2cDriver?.Dispose();
            _gpioConnection?.Close();
        }
    }
}
