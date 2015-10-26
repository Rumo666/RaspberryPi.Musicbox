using System.Web.Http;
using Common.Logging;
using Jukebox.Core;
using Jukebox.Device.Web.Model;

namespace Jukebox.Device.Web.Api
{
    public class PlayerApiController : ApiController
    {
        private static readonly ILog log = LogManager.GetLogger<PlayerApiController>();

        [Route("api/player/toggle")]
        [HttpGet]
        public IHttpActionResult Toggle()
        {
            WebInterface.Controller.TogglePlay();

            return Ok();
        }

        [Route("api/player/next")]
        [HttpGet]
        public IHttpActionResult Next()
        {
            WebInterface.Controller.PlayNext();

            return Ok();
        }

        [Route("api/player/previous")]
        [HttpGet]
        public IHttpActionResult Previous()
        {
            WebInterface.Controller.PlayPrevious();

            return Ok();
        }

        [Route("api/player/volume/{value}")]
        [HttpGet]
        public IHttpActionResult Volume(byte value)
        {
            WebInterface.Controller.SetVolume(value);

            return Ok();
        }

        [Route("api/player/state")]
        [HttpGet]
        public DeviceState State()
        {
            return new DeviceState
            {
                Player = WebInterface.Controller.PlayerStatus,
                LcdLine1 = WebInterface.LcdLine1,
                LcdLine2 = WebInterface.LcdLine2
            };
        }
    }
}
