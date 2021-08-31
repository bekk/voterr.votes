using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Voterr.Votes.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/votes")]
    public class VotesController : ControllerBase
    {
        private readonly ILogger<VotesController> _logger;

        public VotesController(ILogger<VotesController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public object Get()
        {
            return new{ };
        }
    }
}