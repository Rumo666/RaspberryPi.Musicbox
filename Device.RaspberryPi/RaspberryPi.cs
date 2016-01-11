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
        private readonly ScreenManager _screenManager;

        public RaspberryPi(IController controller)
        {
            _controller = controller;
            _screenManager = new ScreenManager();

            // init gpio driver
            var driver = GpioConnectionSettings.DefaultDriver;

            // init connections
            _volumeConnection = new VolumeConnection(driver);
            _lcdConnection = new LcdConnection(driver);
            _btnConnection = new ButtonConnection(controller);
            _rfidConnection = new RfidConnection();
            _ampConnection = new AmplifierConnection();
            _raspiAtxConnection = new RaspiAtxConnection(controller, driver);
        }

        #region IDevice

        public void Initalize()
        {
            // set boot pin to highg
            _raspiAtxConnection.SetBookOk();

            // wake up amp
            _ampConnection.SetShutdownState(false);

            // set amp volume to 0 (because amp its turned to 100% output after wakeup)
            _ampConnection.SetVolume(0);
        }

        public void Shutdown()
        {
            // set on/off button to low
            _raspiAtxConnection.SetOnOffButtonState(false);

            var process = new Process();
            process.StartInfo.FileName = "poweroff";
            process.Start();
        }

        public void ProcessCycle()
        {
            // process volume
            if (_volumeConnection.ReadVolume())
                _controller.SetVolume(_volumeConnection.Volume);

            // process rfid
            _rfidConnection.ReadData(id => _controller.Play(id));

            // render display
            var screen = _screenManager.GetCurrentScreen();
            var content = screen.Render(_lcdConnection);
            _lcdConnection.UpdateDisplay(content.GetRow(0), content.GetRow(1));
        }

        public void ShowScreen(IScreen screen, TimeSpan? timeout)
        {
            _screenManager.SetScreen(screen, timeout);
        }

        public void SetVolume(byte value)
        {
            _ampConnection.SetVolume(value);
        }

        public void InitShutdown()
        {
            _raspiAtxConnection.SetOnOffButtonState(true);
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
