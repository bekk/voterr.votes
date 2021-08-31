using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using Microsoft.Azure.Cosmos;
using Voterr.Votes.Api.Models;

namespace Voterr.Votes.Api.Helpers
{
    public static class CosmosVotesHelpers
    {
        public static async IAsyncEnumerable<Vote> ToAsyncEnumerable(this FeedIterator<Vote> feedIterator, [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            while (feedIterator.HasMoreResults || !cancellationToken.IsCancellationRequested)
            {
                foreach (var vote in await feedIterator.ReadNextAsync(cancellationToken))
                {
                    yield return vote;
                }
            }
        }
    }
}