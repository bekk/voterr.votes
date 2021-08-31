using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using Voterr.Votes.Api.Helpers;
using Voterr.Votes.Api.Models;

namespace Voterr.Votes.Api.Services
{
    public class VotesService
    {
        private readonly CosmosClient _cosmosClient;
        private readonly string _databaseId;
        private readonly string _containerId;

        private readonly ItemRequestOptions _requestOptions = new()
        {
            ConsistencyLevel = ConsistencyLevel.Eventual,
        };

        public VotesService(CosmosClient cosmosClient, string databaseId, string containerId)
        {
            _cosmosClient = cosmosClient;
            _databaseId = databaseId;
            _containerId = containerId;
        }

        public async Task<Vote> CastVote(int candidateId, string userObjectId, string userTenantId, CancellationToken cancellationToken)
        {
            var vote = new Vote()
            {
                CandidateId = candidateId,
                VoterObjectId = userObjectId,
                VoterTenantId = userTenantId
            };
            
            var container = GetContainer();

            var result = await container.CreateItemAsync(vote, PartitionKey.None, _requestOptions, cancellationToken);

            return result.Resource;
        }

        public async Task<List<Vote>> GetVotesByUserIdAndTenantId(string userObjectId, string userTenantId, CancellationToken cancellationToken)
        {
            var query = new QueryDefinition(
                "SELECT * FROM v WHERE v.VoterObjectId = @VoterObjectId AND v.VoterTenantId = @VoterTenantId")
                .WithParameter("@VoterObjectId", userObjectId)
                .WithParameter("@VoterTenantId", userTenantId);

            return await GetContainer()
                .GetItemQueryIterator<Vote>(query)
                .ToAsyncEnumerable(cancellationToken)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<Vote>> GetAllVotes(CancellationToken cancellationToken)
        {
            var query = new QueryDefinition("SELECT * FROM v");

            return await GetContainer()
                .GetItemQueryIterator<Vote>(query)
                .ToAsyncEnumerable(cancellationToken)
                .ToListAsync(cancellationToken);
        }

        private Container GetContainer() => _cosmosClient.GetContainer(_databaseId, _containerId);
    }
}