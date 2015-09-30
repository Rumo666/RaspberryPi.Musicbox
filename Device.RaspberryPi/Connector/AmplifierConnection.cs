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
        private I2cDeviceConnection _connection;
        private I2cDriver _i2cDriver;

        public AmplifierConnection()
        {
            log.Debug(m => m("Init amplifier connection"));

            _i2cDriver = new I2cDriver(ProcessorPin.Pin2, ProcessorPin.Pin3);
            _connection = _i2cDriver.Connect(0x4b);
        }

        public void SetVolume(byte value)
        {
            var data = (byte)(value / 255f * 63);

            log.Debug(m => m("Set amplifier volume to {0}", data));

            _connection.WriteByte(data);
        }

        public void Dispose()
        {
            _i2cDriver?.Dispose();
        }
    }
}
