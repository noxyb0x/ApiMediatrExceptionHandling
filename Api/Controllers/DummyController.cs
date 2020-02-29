using MediatR;
using System.Web.Http;

namespace Api.Controllers
{
    [Route("api/Dummy")]
    public class DummyController : ApiController
    {
        private readonly IMediator _mediator;

        public DummyController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public string CreateDummy([FromBody]string value)
        {
            // todo: send test command
            return "Pong";
        }

        [HttpGet]
        public string GetDummy()
        {
            // todo: send test query
            return "value";
        }
    }
}
