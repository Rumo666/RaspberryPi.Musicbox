using System;
using System.Threading;
using Common.Logging;
using Jukebox.Core;
using Jukebox.Core.Renderer;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test
{
    [TestClass]
    public class DisplayTest
    {
        private class MockDisplay : IDisplay
        {
            public int Columns { get; } = 16;
            public int Rows { get; } = 20;
        }

        private static readonly ILog log = LogManager.GetLogger<DisplayTest>();

        [TestMethod]
        public void RendererScroll()
        {
            var end = DateTime.Now.Add(new TimeSpan(0, 0, 0, 30));
            var filter = new ScrollRenderer(ScrollRenderer.Modus.Infinite, new TimeSpan(0, 0, 0, 0, 100), new TimeSpan(0, 0, 3));
            var display = new MockDisplay();
            var content = "Das ist ein langer Text der zum Testen dienen soll und nichts sinnvolles enthält";
            //var content = "Das ist ein langer Text";

            while (true)
            {
                var output = filter.Render(content, display);

                log.Debug($"[{output}]");

                Thread.Sleep(100);

                if (end < DateTime.Now)
                    break;
            }

        }

        [TestMethod]
        public void RendererLineBreak()
        {
            var filter = new LineBreakRenderer();
            var display = new MockDisplay();
            var content = "Das ist ein langer Text der zum Testen dienen soll und nichts sinnvolles enthält aber umbrechen sollte";

            var output = filter.Render(content, display);

            foreach (var row in output.Rows)
            {
                log.Debug(row);
            }
        }
    }
}
