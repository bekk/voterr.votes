using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using Microsoft.FeatureManagement;

namespace Voterr.Votes.Api
{
	public class CosmosDbClient
	{
		private readonly CosmosClient _cosmosClient;

		public CosmosDbClient(IConfiguration configuration, IFeatureManager featureManager)
		{
			if (featureManager.IsEnabledAsync(FeatureFlags.UseManagedIdentity).GetAwaiter().GetResult())
			{

			}
			else
			{
				_cosmosClient = new CosmosClient(configuration.GetConnectionString("Cosmos"));
			}
		}
		public Container GetContainer(string databaseId, string containerId)
		{
			return _cosmosClient.GetContainer(databaseId, containerId);
		}
	}
}
