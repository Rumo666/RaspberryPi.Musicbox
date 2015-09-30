using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jukebox.Core
{
    public class VirtualDisplay
    {
        public struct Content
        {
            public string Line1 { get; set; }
            public string Line2 { get; set; }
            public DateTime? End { get; set; }
        }

        private int _columns { get; }
        private Content _timedEntry { get; set; }
        private Content _stayEntry { get; set; }

        public VirtualDisplay(int columns)
        {
            _columns = columns;
        }

        public void SetContent(string line1, string line2, TimeSpan? timeout = null)
        {
            var entry = new Content
            {
                Line1 = line1,
                Line2 = line2,
                End = (timeout != null ? (DateTime?)DateTime.Now.Add(timeout.Value) : null)
            };

            if (timeout == null)
                _stayEntry = entry;
            else
                _timedEntry = entry;
        }

        public Content GetContent()
        {
            return (_timedEntry.End < DateTime.Now
                ? _stayEntry
                : _timedEntry);
        }
    }
}
