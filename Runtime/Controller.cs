using Common.Logging;
using Jukebox.Core;
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
        private string _currentTagId;
        private byte _currentVolume;
        private const byte _maxVolume = 140;
        private bool _initalized;
        private DateTime _lastLcdUpdate;

        private const byte _lcdCharacterPlay = 0x0;
        private const byte _lcdCharacterPause = 0x1;
        private const byte _lcdCharacterStop = 0x2;

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
            // prevent volume bobbing
            if (value < _currentVolume + 10 && value > _currentVolume - 10)
                return;

            _currentVolume = value;
            var percentage = value / 255f;
            var volume = (byte)(_maxVolume * percentage);

            log.Info(m => m($"Set volume to {volume}"));

            Do(dev => dev.SetVolume(volume));

            // set display
            if (_initalized)
                SetLcdText($"Volume {(int)(percentage * 100)}%", "", new TimeSpan(0, 0, 0, 2));
        }

        public PlayerStatus GetPlayerState()
        {
            return _player.GetStatus();
        }

        public void Shutdown()
        {
            if (!_initalized)
                return;

            log.Info(m => m("Shutdown system triggered"));

            SetLcdText("Shutdown...", "Bye bye..");

            _player.Stop();

            // trigger shutdown
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

            // show album
        }

        public void Process()
        {
            // update lcd every second (to prevent exceptions)
            if (_lastLcdUpdate.AddSeconds(1) < DateTime.Now)
            {
                SetLcdWithCurrentState();
                _lastLcdUpdate = DateTime.Now;
            }

            // process devices
            Do(device => device.Process());
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

            SetLcdWithCurrentState();

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

            Do(device => device.DisplayText(line1, line2, timeout));
        }

        private void SetLcdWithCurrentState()
        {
            var status = GetPlayerState();

            var state = (status.State == PlayerStatus.States.Play 
                ? _lcdCharacterPlay
                : status.State == PlayerStatus.States.Pause
                    ? _lcdCharacterPause
                    : _lcdCharacterStop);
            

            var line1 = $"{(char)state} {status.TrackNumber}/{status.PlaylistLength} {(int)status.CurrentPosition.TotalMinutes:00}:{status.CurrentPosition.Seconds:00}";
            var line2 = $"{status.Title}";

            SetLcdText(line1, line2);
        }
    }
}
