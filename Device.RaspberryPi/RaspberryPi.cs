using System;
using Jukebox.Core;
using Raspberry.IO.GeneralPurpose;
using Jukebox.Device.RaspberryPi.Connector;
using System.Diagnostics;

namespace Jukebox.Device.RaspberryPi
{
    public class RaspberryPi : IDevice, IDisposable
    {
        private readonly IController _controller;
        private readonly VolumeConnection _volumeConnection;
        private readonly LcdConnection _lcdConnection;
        private readonly ButtonConnection _btnConnection;
        private readonly RfidConnection _rfidConnection;
        private readonly AmplifierConnection _ampConnection;
        private readonly RaspiAtxConnection _raspiAtxConnection;
        private readonly VirtualDisplay _virtualDisplay;

        public RaspberryPi(IController controller)
        {
            _controller = controller;
            _virtualDisplay = new VirtualDisplay(16);

            // init gpio driver
            var driver = GpioConnectionSettings.DefaultDriver;

            // init connections
            _volumeConnection = new VolumeConnection(driver);
            _lcdConnection = new LcdConnection(driver);
            _btnConnection = new ButtonConnection(controller);
            _rfidConnection = new RfidConnection();
            _ampConnection = new AmplifierConnection();
            _raspiAtxConnection = new RaspiAtxConnection(controller);
        }

        #region IDevice

        public void Process()
        {
            // process volume
            if (_volumeConnection.ReadVolume())
                _controller.SetVolume(_volumeConnection.Volume);

            // process rfid
            _rfidConnection.ReadData(id => _controller.PlayByTagId(id));

            // update display
            var content = _virtualDisplay.GetContent();
            _lcdConnection.UpdateDisplay(content.Line1, content.Line2);
        }

        public void DisplayText(string line1, string line2, TimeSpan? timeout)
        {
            _virtualDisplay.SetContent(line1, line2, timeout);
        }

        public void SetVolume(byte value)
        {
            _ampConnection.SetVolume(value);
        }

        public void Initalize()
        {
            // set boot pin to highg
            _raspiAtxConnection.SetBookOk();
        }

        public void Shutdown()
        {
            var process = new Process();
            process.StartInfo.FileName = "poweroff";
            process.Start();
        }

        #endregion

        #region IDisposable

        public void Dispose()
        {
            _volumeConnection?.Dispose();
            _lcdConnection?.Dispose();
            _btnConnection?.Dispose();
            _rfidConnection?.Dispose();
            _ampConnection?.Dispose();
            _raspiAtxConnection?.Dispose();
        }

        #endregion
    }
}
