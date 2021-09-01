using System;
using System.Collections.Generic;
using System.Linq;
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
        private const string DatabaseId = "Voterr";
        private const string ContainerId = "Votes";

        private readonly ItemRequestOptions _requestOptions = new()
        {
            ConsistencyLevel = ConsistencyLevel.Eventual,
        };

        public VotesService(CosmosClient cosmosClient)
        {
            _cosmosClient = cosmosClient;
        }

        public async Task<Vote> CastVote(int candidateId, string userObjectId, string userTenantId, CancellationToken cancellationToken)
        {
            var vote = new Vote()
            {
                Id = Guid.NewGuid().ToString(),
                CandidateId = candidateId,
                VoterObjectId = userObjectId,
                VoterTenantId = userTenantId,
                Timestamp = DateTime.Now,
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

        public async Task<List<CandidateVotes>> GetVotingResults(CancellationToken cancellationToken)
        {
            var query = new QueryDefinition("SELECT * FROM v");

            var results = await GetContainer()
                .GetItemQueryIterator<Vote>(query)
                .ToAsyncEnumerable(cancellationToken)
                .ToListAsync(cancellationToken);

            return results
                .GroupBy(v => v.CandidateId)
                .Select(result => new CandidateVotes() {CandidateId = result.Key, VoteCount = result.Count()})
                .ToList();
        }

        private Container GetContainer() => _cosmosClient.GetContainer(DatabaseId, ContainerId);
    }
}