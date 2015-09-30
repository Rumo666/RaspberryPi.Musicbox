using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Logging;
using Jukebox.Core;

namespace Jukebox.Device.RaspberryPi.Connector
{
    public class RfidConnection : IDisposable
    {
        private static readonly ILog log = LogManager.GetLogger<RfidConnection>();
        private readonly SerialPort _serial;
        private byte[] _buffer = new byte[12];
        private byte _position;
        private DateTime? _lastRead;
        private readonly TimeSpan _minReadIntervall = new TimeSpan(0,0,0,1);

        public RfidConnection()
        {
            log.Debug(m => m("Init RFID connection"));

            _serial = new SerialPort("/dev/ttyAMA0", 9600/*, Parity.None, 8, StopBits.One*/);

            if (!_serial.IsOpen)
                _serial.Open();
        }

        public void ReadData(Action<string> action)
        {
            // read buffered serial data
            for (int i = 0; i < _serial.BytesToRead; i++)
            {
                var dataByte = (byte)_serial.ReadByte();

                switch (dataByte)
                {
                    // start byte
                    case 0x02:
                        _buffer = new byte[12];
                        _position = 0;
                        break;

                    // end byte
                    case 0x03:
                        ProcessData(action);
                        break;

                    // data byte
                    default:
                        _buffer[_position] = dataByte;
                        _position++;
                        break;
                }
            }
        }

        private void ProcessData(Action<string> action)
        {
            // convert buffer
            var dataAscii = Encoding.ASCII.GetString(_buffer.Take(10).ToArray());
            var dataHex = dataAscii.ConvertHexStringToByteArray();

            log.Debug(m => m("Serial buffer: data: {0}, length: {1}, hex: {2}", BitConverter.ToString(_buffer), _position, BitConverter.ToString(dataHex)));

            // validate
            var checksumRecived = Encoding.ASCII.GetString(_buffer.Skip(10).Take(2).ToArray()).ConvertHexStringToByteArray()[0];
            var checksum = dataHex.Aggregate((a, b) => (byte)(a ^ b));

            if (checksum != checksumRecived)
            {
                log.Debug(m => m("Invalid RFID tag checksum (calculated: {0}, recived: {1})", checksum, _buffer[10]));
                return;
            }
            
            // check min intervall between read
            if (_lastRead > DateTime.Now - _minReadIntervall)
                return;

            // mark last read
            _lastRead = DateTime.Now;
            
            // convert to int
            var number = int.Parse(dataAscii.Substring(4), NumberStyles.HexNumber).ToString();

            log.Info(m => m("Detect RFID tag '{0}'", number));

            action.Invoke(number);

            //if (cnt < 10)
            //{
            //    tag += BitConverter.ToChar(data, 0);
            //    if (cnt > 3)
            //        tagBuffer[cnt - 4] = data[0];
            //    cnt++;
            //}
            //else
            //{
            //    if (!tagRead)
            //    {
            //        {
            //            tag = tag.Substring(4);
            //            var num = int.Parse(tag, NumberStyles.HexNumber);
            //            Console.WriteLine("Tag: {0} - {1}", tag, num);
            //        }
            //    }
            //}
        }

        public void Dispose()
        {
        }
    }
}
