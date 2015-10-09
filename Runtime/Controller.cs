using Common.Logging;
using Jukebox.Core;
using Jukebox.Core.Screens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jukebox.Runtime
{
    public class Controller : IController
    {
        private static readonly ILog log = LogManager.GetLogger<Controller>();

        private IEnumerable<IDevice> _devices;
        private readonly IPlayer _player;
        private int _currentSongId;
        private string _currentTagId;
        private byte _currentVolume;
        private const byte _minVolume = 40;
        private const byte _maxVolume = 120;
        private bool _initalized;
        private MainScreen _mainScreen = new MainScreen();

        public Controller(IPlayer player)
        {
            _player = player;
        }

        #region IController

        public bool IsShuttingDown
        {
            get; private set;
        }

        public void TogglePlay()
        {
            if (!_initalized)
                return;

            log.Info(m => m("Toggle play"));

            var status = _player.GetStatus();

            if (status.State == PlayerStatus.States.Play)
            {
                _player.Pause(true);
            }
            else if (status.State == PlayerStatus.States.Pause)
            {
                _player.Pause(false);
            }
            else
            {
                _player.Play();
            }
        }

        public void PlayNext()
        {
            if (!_initalized)
                return;

            log.Info(m => m("Play next song"));

            _player.Next();
        }

        public void PlayPrevious()
        {
            if (!_initalized)
                return;

            log.Info(m => m("Play previous song"));

            _player.Previous();
        }

        public void SetVolume(byte value)
        {
            if (!_initalized)
                return;

            // prevent volume wobble
            if (value < _currentVolume + 3 && value > _currentVolume - 3)
                return;

            _currentVolume = value;
            var percentage = value / 255f;
            var volume = (byte)((_maxVolume - _minVolume) * percentage + _minVolume);

            log.Info(m => m($"Set volume to {volume}"));

            Do(dev => dev.SetVolume(volume));

            // set display
            SetLcdText($"Volume {(int)(percentage * 100)}%", "", new TimeSpan(0, 0, 0, 2));
        }

        public PlayerStatus GetPlayerStatus()
        {
            return _player.GetStatus();
        }

        public void Shutdown()
        {
            // do not shutdown before initalized
            if (!_initalized || IsShuttingDown)
                return;

            log.Info(m => m("Shutdown system triggered"));

            // stop playing
            _player.Stop();

            // trigger shutdown on devices
            Do(dev => dev.Shutdown());

            IsShuttingDown = true;
        }

        public void PlayByTagId(string id)
        {
            log.Info(m => m("Play by tag id '{0}'", id));

            // ignore if its same as current tag
            if (_currentTagId == id)
            {
                log.Debug(m => m("Ignore tag because its already playing"));

                return;
            }

            // play by id
            _player.PlayByTagId(id);

            // remember current tag
            _currentTagId = id;
        }

        public void ProcessCycle()
        {
            var status = GetPlayerStatus();

            // has song changed
            if (_currentSongId != status.SongId)
            {
                _mainScreen.Reset();

                _currentSongId = status.SongId;
            }

            // update main screen
            _mainScreen.PlayerStatus = status;
            Do(device => device.ShowScreen(_mainScreen, null));

            // process devices
            Do(device => device.ProcessCycle());
        }

        #endregion

        public void Initalize(IEnumerable<IDevice> devices)
        {
            log.Debug("Initalize");

            _devices = devices;

            // initalize devices
            Do(dev => dev.Initalize());

            // get current player state
            var state = _player.GetStatus();

            _currentTagId = state.TagId;

            SetLcdText("Hallo Noah", "", new TimeSpan(0, 0, 2));

            _initalized = true;
        }

        private void Do(Action<IDevice> action)
        {
            if (_devices == null)
            {
                log.Warn("Ignore action, devices are not yet inizalized");
                return;
            }

            foreach (var device in _devices)
            {
                action.Invoke(device);
            }
        }

        private void SetLcdText(string line1, string line2, TimeSpan? timeout = null)
        {
            log.Debug(m => m("Update LCD (line1: '{0}', line2: '{1}', timeout: {2}", line1, line2, timeout));

            Do(device => device.ShowScreen(new GenericScreen(line1, line2), timeout));
        }
    }
}
