using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement;
using Voterr.Votes.Api.Services;

namespace Voterr.Votes.Api.Controllers
{
	[Route("api/status")]
	public class StatusController : ControllerBase
	{
		private readonly IFeatureManager _featureManager;
		private readonly VotesService _votesService;

		public StatusController(IFeatureManager featureManager, VotesService votesService)
		{
			_featureManager = featureManager;
			_votesService = votesService;
		}

		[HttpGet]
		public async Task<IActionResult> Get()
		{
			var managedIdentityEnabled = await _featureManager.IsEnabledAsync(FeatureFlags.UseManagedIdentity);
			var allVotes = await _votesService.GetAllVotes(CancellationToken.None);

			return Ok(new
			{
				managedIdentityEnabled,
				numberOfVotesInDb = allVotes.Count
			});
		}
	}
}
