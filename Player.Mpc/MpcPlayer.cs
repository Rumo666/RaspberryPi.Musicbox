using Common.Logging;
using Jukebox.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Libmpc;

namespace Jukebox.Player.Mpc
{
    public class MpcPlayer : IPlayer
    {
        private static readonly ILog log = LogManager.GetLogger<MpcPlayer>();
        private static readonly object _lock = new object();

        private readonly Libmpc.Mpc _mpc;

        public MpcPlayer(IPEndPoint endpoint)
        {
            log.Debug(m => m("Connect to MPD"));

            _mpc = new Libmpc.Mpc
            {
                Connection = new MpcConnection
                {
                    Server = endpoint,
                    AutoConnect = true
                }
            };
        }

        #region IPlayer

        public void Next()
        {
            log.Debug(m => m("Send MPD command: next"));

            lock (_lock)
            {
                _mpc.Next();
            }
        }

        public void Previous()
        {
            log.Debug(m => m("Send MPD command: previous"));

            lock (_lock)
            {
                _mpc.Previous();
            }
        }

        public void Stop()
        {
            log.Debug(m => m("Send MPD command: stop"));

            lock (_lock)
            {
                _mpc.Stop();
            }
        }

        public void Play()
        {
            log.Debug(m => m("Send MPD command: play"));

            lock (_lock)
            {
                _mpc.Play();
            }
        }

        public void Play(string id)
        {
            lock (_lock)
            {
                // find songs by card id
                var songs = _mpc.Find(ScopeSpecifier.Comment, id);

                log.Debug(m => m("Found {0} songs by comment '{1}'", songs.Count, id));

                // clear current playlist
                log.Debug(m => m("Send MPD command: clear playlist"));

                _mpc.Clear();

                // add songs to playlist
                foreach (var song in songs)
                {
                    log.Debug(m => m("Send MPD command: add song '{0}' to playlist", song.File));

                    _mpc.Add(song.File);
                }
            }

            Play();
        }

        public void SetVolume(byte volume)
        {
            log.Debug(m => m("Send MPD command: setVol {0}", volume));

            lock (_lock)
            {
                _mpc.SetVol((int)volume);
            }
        }

        public void Pause(bool pause)
        {
            log.Debug(m => m("Send MPD command: pause ({0})", pause));

            lock (_lock)
            {
                _mpc.Pause(pause);
            }
        }

        public PlayerStatus GetStatus()
        {
            lock (_lock)
            {
                var status = _mpc.Status();
                var song = _mpc.CurrentSong();

                log.Debug($"Get MPD state (state: {status.State}, album: '{song?.Album}', title: '{song?.Title}', artist: '{song?.Artist}', tagId: '{song?.Comment}', songId: {song?.Id})");

                var state = PlayerStatus.States.Play;
                switch (status.State)
                {
                    case MpdState.Pause:
                        state = PlayerStatus.States.Pause;
                        break;
                    case MpdState.Stop:
                        state = PlayerStatus.States.Stop;
                        break;
                }

                return new PlayerStatus
                {
                    State = state,
                    Volume = (byte)status.Volume,
                    Album = song?.Album,
                    Title = song?.Title,
                    Artist = song?.Artist,
                    TagId = song?.Comment,
                    TrackNumber = Convert.ToInt32(song?.Track),
                    PlaylistLength = status.PlaylistLength,
                    CurrentPosition = new TimeSpan(0, 0, status.TimeElapsed),
                    SongId = song?.Id ?? 0
                };
            }
        }

        #endregion
    }
}
