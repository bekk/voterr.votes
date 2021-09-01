using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.Resource;
using Voterr.Votes.Api.Authorization;
using Voterr.Votes.Api.Dtos;
using Voterr.Votes.Api.Models;
using Voterr.Votes.Api.Services;

namespace Voterr.Votes.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/votes")]
    public class VotesController : ControllerBase
    {
        private readonly ILogger<VotesController> _logger;
        private readonly VotesService _votesService;

        public VotesController(ILogger<VotesController> logger, VotesService votesService)
        {
            _logger = logger;
            _votesService = votesService;
        }

        [HttpPost]
        [RequiredScope(Scopes.VotesCast)]
        public async Task<IActionResult> CastVote(VoteDto vote, CancellationToken cancellationToken)
        {
            var displayName = User.GetDisplayName();
            var userObjectId = User.GetObjectId();
            var userTenantId = User.GetTenantId();

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("A sneaky user with name {DisplayName}, tried to do something funny");
                return BadRequest(ModelState);
            }
            
            _logger.LogInformation("Casted vote for {CandidateId} by {DisplayName}, ObjectId {ObjectId} and TenantId {TenantId}", vote.CandidateId.ToString(), displayName, userObjectId, userTenantId);
            return Ok(await _votesService.CastVote(vote.CandidateId, userObjectId, userTenantId, cancellationToken));
        }

        [HttpGet("my")]
        [RequiredScope(Scopes.VotesReadMine)]
        public async Task<IEnumerable<Vote>> GetMyVotes(CancellationToken cancellationToken)
        {
            var displayName = User.GetDisplayName();
            var userObjectId = User.GetObjectId();
            var userTenantId = User.GetTenantId();
            
            _logger.LogInformation("Retrieving votes casted by {DisplayName}, ObjectId {ObjectId} and TenantId {TenantId}", displayName, userObjectId, userTenantId);
            return await _votesService.GetVotesByUserIdAndTenantId(userObjectId, userTenantId, cancellationToken);
        }

        [HttpGet("results")]
        [RequiredScope(Scopes.VotesReadResult)]
        public async Task<IEnumerable<CandidateVotes>> GetResults(CancellationToken cancellationToken)
        {
            var displayName = User.GetDisplayName();
            var userObjectId = User.GetObjectId();
            var userTenantId = User.GetTenantId();
            
            _logger.LogInformation("Retrieving results for {DisplayName}, ObjectId {ObjectId} and TenantId {TenantId}", displayName, userObjectId, userTenantId);
            return await _votesService.GetVotingResults(cancellationToken);
        }

        [HttpGet("all")]
        [RequiredScope(Scopes.VotesReadAll)]
        public async Task<IEnumerable<Vote>> GetAllVotes(CancellationToken cancellationToken)
        {
            var displayName = User.GetDisplayName();
            var userObjectId = User.GetObjectId();
            var userTenantId = User.GetTenantId();
            
            _logger.LogInformation("Retrieving all votes. Requested by {DisplayName}, ObjectId {ObjectId} and TenantId {TenantId}", displayName, userObjectId, userTenantId);
            return await _votesService.GetAllVotes(cancellationToken);
        }
    }
}