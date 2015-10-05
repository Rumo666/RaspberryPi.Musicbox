using System;
using System.Threading;
using Common.Logging;
using Jukebox.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test
{
    [TestClass]
    public class DisplayTest
    {
        private class MockDisplay : IDisplay
        {
            public int Columns { get; } = 16;
            public int Rows { get; } = 2;
        }

        private static readonly ILog log = LogManager.GetLogger<DisplayTest>();

        [TestMethod]
        public void ScreenScrollFilter()
        {
            var end = DateTime.Now.Add(new TimeSpan(0, 0, 0, 30));
            var filter = new ScreenFilterScroll(new TimeSpan(0, 0, 0, 0, 100), new TimeSpan(0, 0, 3));
            var display = new MockDisplay();
            var content = "Das ist ein langer Text der zum Testen dienen soll und nichts sinnvolles enthält";

            while (true)
            {
                var output = filter.Render(content, display);

                log.Debug($"[{output}]");

                Thread.Sleep(100);

                if (end < DateTime.Now)
                    break;
            }

        }
    }
}
