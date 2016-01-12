﻿using Common.Logging;
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
        private string _currentAlbumId;
        private byte _currentVolume;
        private const byte _minVolume = 40;
        private const byte _maxVolume = 120;
        private bool _initalized;
        private readonly MainScreen _mainScreen = new MainScreen();
        private PlayerStatus _playerStatus;
        private DateTime _lastPlayingTime;
        private readonly TimeSpan _maxIdleTime = new TimeSpan(0, 5, 0);
        private DateTime _lastProcessCycle;

        public Controller(IPlayer player)
        {
            _player = player;
            _lastPlayingTime = DateTime.Now;
            _lastProcessCycle = DateTime.Now;
        }

        #region IController

        public PlayerStatus PlayerStatus
        {
            get
            {
                if (_playerStatus == null)
                    UpdatePlayerStatus();

                return _playerStatus;
            }
        }

        public bool IsShuttingDown
        {
            get; private set;
        }

        public void TogglePlay()
        {
            if (!_initalized)
                return;

            log.Info(m => m("Toggle play"));

            if (PlayerStatus.State == PlayerStatus.States.Play)
            {
                _player.Pause(true);
            }
            else if (PlayerStatus.State == PlayerStatus.States.Pause)
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

            // do not play next when last song is playing
            if (PlayerStatus.TrackNumber == PlayerStatus.PlaylistLength)
                return;

            log.Info(m => m("Play next song"));

            _player.PlayNext();
        }

        public void PlayPrevious()
        {
            if (!_initalized)
                return;

            log.Info(m => m("Play previous song"));

            _player.PlayPrevious();
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

            log.Debug(m => m($"Set volume to {volume}"));

            Do(dev => dev.SetVolume(volume));

            // set display
            //SetLcdText(new[] { $"Volume {(int)(percentage * 100)}%" }, new TimeSpan(0, 0, 0, 2));
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

        public void Play(string id)
        {
            log.Info(m => m("Play by tag id '{0}'", id));

            // is current album already playing
            if (_currentAlbumId == id)
            {
                // don't go to first track, if playing recently started
                if (PlayerStatus.TrackNumber == 1 && PlayerStatus.CurrentPosition < new TimeSpan(0, 0, 10))
                    return;
                
                // go back to first track
                _player.PlayFirst();

                return;
            }

            // play by id
            _player.Play(id);
        }

        public void ProcessCycle()
        {
            // if last process cycle exceed 'maxIdleTimeout', the system seems slept
            if (_lastProcessCycle < DateTime.Now - _maxIdleTime)
            {
                log.Info("Last process cycle exceed idle timeout");

                // reset last playing time, to prevent imediate shutdown
                _lastPlayingTime = DateTime.Now;
            }

            // update player status
            UpdatePlayerStatus();

            // has song changed
            if (_currentSongId != PlayerStatus.SongId)
            {
                _mainScreen.Reset();

                _currentSongId = PlayerStatus.SongId;
            }

            // has album changes
            if (_currentAlbumId != PlayerStatus.TagId)
            {
                var albumScreen = new AlbumScreen(PlayerStatus);

                Do(device => device.ShowScreen(albumScreen, new TimeSpan(0, 0, 0, 3)));

                _currentAlbumId = PlayerStatus.TagId;
            }

            // update main screen
            _mainScreen.PlayerStatus = PlayerStatus;
            Do(device => device.ShowScreen(_mainScreen, null));

            // set idle time
            if (PlayerStatus.State == PlayerStatus.States.Play)
                _lastPlayingTime = DateTime.Now;

            // shutdown if idle timeout reached
            if (_lastPlayingTime + _maxIdleTime < DateTime.Now)
            {
                log.Info(m => m($"Reach idle timeout, init shutdown (lpt: {_lastPlayingTime}, lpc: {_lastProcessCycle})"));

                Do(device => device.InitShutdown());
            }

            // process devices
            Do(device => device.ProcessCycle());

            _lastProcessCycle = DateTime.Now;
        }

        #endregion

        public void Initalize(IEnumerable<IDevice> devices)
        {
            log.Debug(m => m("Initalize"));

            _devices = devices;

            // initalize devices
            Do(dev => dev.Initalize());

            // get current player state
            var state = _player.GetStatus();

            _currentAlbumId = state.TagId;

            SetLcdText(new[] { "Hallo Noah" }, new TimeSpan(0, 0, 2));

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

        private void SetLcdText(IEnumerable<string> content, TimeSpan? timeout = null)
        {
            var screenContent = new ScreenContent(content);

            log.Debug(m => m("Update LCD (line1: '{0}', line2: '{1}', timeout: {2}", screenContent.GetRow(0), screenContent.GetRow(1), timeout));

            Do(device => device.ShowScreen(new GenericScreen(screenContent), timeout));
        }

        private void UpdatePlayerStatus()
        {
            _playerStatus = _player.GetStatus();
        }
    }
}
