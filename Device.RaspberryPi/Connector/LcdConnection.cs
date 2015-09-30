using Common.Logging;
using Raspberry.IO;
using Raspberry.IO.Components.Displays.Hd44780;
using Raspberry.IO.GeneralPurpose;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jukebox.Device.RaspberryPi.Connector
{
    public class LcdConnection : IDisposable
    {
        private static readonly ILog log = LogManager.GetLogger<LcdConnection>();
        private static readonly object _lockject = new object();

        private readonly Hd44780LcdConnection _lcdConnection;
        private string _line1;
        private string _line2;

        public LcdConnection(IGpioConnectionDriver driver)
        {
            log.Debug(m => m("Init LCD connection"));

            var lcdSettings = new Hd44780LcdConnectionSettings
            {
                ScreenWidth = 16,
                ScreenHeight = 2,
                //Encoding = Encoding.ASCII
            };

            var dataPins = new[]
                {
                    ConnectorPin.P1Pin31,
                    ConnectorPin.P1Pin33,
                    ConnectorPin.P1Pin35,
                    ConnectorPin.P1Pin37
                }
                .Select(p => (IOutputBinaryPin)driver.Out(p))
                .ToArray();

            var lcdPins = new Hd44780Pins(
                driver.Out(ConnectorPin.P1Pin40),
                driver.Out(ConnectorPin.P1Pin38),
                dataPins);

            _lcdConnection = new Hd44780LcdConnection(lcdSettings, lcdPins);

            // init additonal characters
            _lcdConnection.SetCustomCharacter(0x0, new byte[] { 0x8, 0xc, 0xe, 0xf, 0xe, 0xc, 0x8 });       // play
            _lcdConnection.SetCustomCharacter(0x1, new byte[] { 0x0, 0x1b, 0x1b, 0x1b, 0x1b, 0x1b, 0x0 });  // pause
            _lcdConnection.SetCustomCharacter(0x2, new byte[] { 0x0, 0x1f, 0x1f, 0x1f, 0x1f, 0x1f, 0x0 });  // stop

        }

        public void UpdateDisplay(string line1, string line2)
        {
            // skip update if content is the same
            if (line1 == _line1 && line2 == _line2)
                return;

            _line1 = line1;
            _line2 = line2;

            lock (_lockject)
            {
                _lcdConnection.Clear();
                _lcdConnection.WriteLine(line1);
                _lcdConnection.WriteLine(line2);
            }
        }

        public void Dispose()
        {
            _lcdConnection?.Close();
        }
    }
}
