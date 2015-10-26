using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jukebox.Core
{
    public class ScreenContent
    {
        public const byte LcdCharacterPlay = 0x0;
        public const byte LcdCharacterPause = 0x1;
        public const byte LcdCharacterStop = 0x2;

        private readonly List<string> _rows = new List<string>();

        public ScreenContent()
        {
        }

        public ScreenContent(IEnumerable<string> rows)
        {
            _rows.AddRange(rows);
        }

        public IEnumerable<string> Rows => _rows;
        public int LineCount => _rows.Count;

        public void AddRow(string content)
        {
            _rows.Add(content);
        }

        public string GetRow(int index)
        {
            if (index >= _rows.Count)
                return null;

            return _rows[index];
        }

        public void Clear()
        {
            _rows.Clear();
        }
    }
}
