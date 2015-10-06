using Common.Logging;
using Raspberry.IO.GeneralPurpose;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jukebox.Device.RaspberryPi.Connector
{
    public class VolumeConnection : IDisposable
    {
        private static readonly ILog log = LogManager.GetLogger<VolumeConnection>();

        private readonly Mcp3202SpiConnection _mcpConnection;
        private readonly decimal[] _buffer = new decimal[4];
        private int _bufferIndex = 0;

        public VolumeConnection(IGpioConnectionDriver driver)
        {
            log.Debug(m => m("Init volume connection"));

            _mcpConnection = new Mcp3202SpiConnection(
                driver.Out(ConnectorPin.P1Pin23),
                driver.Out(ConnectorPin.P1Pin24),
                driver.In(ConnectorPin.P1Pin21),
                driver.Out(ConnectorPin.P1Pin19),
                8191);
        }

        public byte Volume { get; private set; }

        public bool ReadVolume()
        {
            var currentValue = Volume;
            
            var value = _mcpConnection.Read(Mcp3202SpiConnection.Channel.A);

            // fill buffer
            _buffer[_bufferIndex++] = value.Relative;
            if (_bufferIndex == _buffer.Length)
                _bufferIndex = 0;
            
            // calc avg
            var avgValue = _buffer.Average();

            //log.Info($"rawValue: {value.Relative}, avgValue: {avgValue}");

            Volume = (byte)((int)(32 * avgValue) / 32f * 255);

            return currentValue != Volume;
        }

        public void Dispose()
        {
            if (_mcpConnection != null)
                _mcpConnection.Dispose();
        }
    }
}
