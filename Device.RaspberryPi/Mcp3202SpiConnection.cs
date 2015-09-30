using System;
using Raspberry.IO.SerialPeripheralInterface;
using Raspberry.IO;

namespace Jukebox.Device.RaspberryPi
{
    public class Mcp3202SpiConnection : IDisposable
    {
        #region Enum

        public enum Channel
        {
            A,
            B
        }

        #endregion

        #region Fields

        private readonly SpiConnection _spiConnection;
        private readonly decimal _maxValue;

        #endregion

        #region Instance Management

        /// <summary>
        /// Initializes a new instance of the <see cref="Mcp3202SpiConnection"/> class.
        /// </summary>
        /// <param name="clockPin">The clock pin.</param>
        /// <param name="slaveSelectPin">The slave select pin.</param>
        /// <param name="misoPin">The miso pin.</param>
        /// <param name="mosiPin">The mosi pin.</param>
        public Mcp3202SpiConnection(IOutputBinaryPin clockPin, IOutputBinaryPin slaveSelectPin, IInputBinaryPin misoPin, IOutputBinaryPin mosiPin, decimal maxValue = 1m)
        {
            _spiConnection = new SpiConnection(clockPin, slaveSelectPin, misoPin, mosiPin, Endianness.LittleEndian);
            _maxValue = maxValue;
        }

        public void Dispose()
        {
            if (_spiConnection != null)
                _spiConnection.Close();
        }

        #endregion

        #region Methods

        public AnalogValue Read(Channel channel)
        {
            using (_spiConnection.SelectSlave())
            {
                // start bit
                _spiConnection.Write(true);

                // sgl/diff
                _spiConnection.Write(true);

                // odd/sign (channel)
                _spiConnection.Write(channel == Channel.B);

                // msbf
                _spiConnection.Write(true);

                // read 13 bits
                var data = (int)_spiConnection.Read(13);

                return new AnalogValue(data, _maxValue);
            }
        }

        #endregion
    }
}
