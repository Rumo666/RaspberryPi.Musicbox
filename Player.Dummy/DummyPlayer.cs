using System;
using System.Collections.Generic;
using Common.Logging;
using Jukebox.Core;

namespace Jukebox.Player.Dummy
{
    public class DummyPlayer : IPlayer
    {
        private class Track
        {
            public string Title { get; set; }
            public string Album { get; set; }
            public string Interpret { get; set; }
        }

        
        private bool _isPlaying;
        private byte _volume;
        private int _index;

        private readonly List<Track> _tracks = new List<Track>(new[]
        {
            new Track {Title = "Track 1", Album = "Album 1", Interpret = "Interpret 1"},
            new Track {Title = "Track 2", Album = "Album 1", Interpret = "Interpret 1"},
            new Track {Title = "Track 3", Album = "Album 1", Interpret = "Interpret 1"},
            new Track {Title = "Track 4", Album = "Album 1", Interpret = "Interpret 1"},
            new Track {Title = "Track 1", Album = "Album 2", Interpret = "Interpret 2"},
            new Track {Title = "Track 2", Album = "Album 2", Interpret = "Interpret 2"}
        });

        #region IPlayer

        public void Next()
        {
            _index = Math.Min(_index + 1, _tracks.Count - 1);
        }

        public void Previous()
        {
            _index = Math.Max(_index - 1, 0);
        }

        public void Stop()
        {
            _isPlaying = false;
        }

        public void Play()
        {
            _isPlaying = true;
        }

        public void Play(string id)
        {
            
        }

        public void Pause(bool pause)
        {
            _isPlaying = !pause;
        }

        public void SetVolume(byte volume)
        {
            _volume = volume;
        }

        public PlayerStatus GetStatus()
        {
            var track = _tracks[_index];

            return new PlayerStatus
            {
                State = (_isPlaying ? PlayerStatus.States.Play : PlayerStatus.States.Stop),
                Volume = _volume,
                Album = track.Album,
                Artist = track.Interpret,
                Title = track.Title
            };
        }

        #endregion

    }
}
