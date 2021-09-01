using Microsoft.FeatureManagement;

namespace Voterr.Votes.Api.Extensions
{
	internal static class FeatureManagerExtension
	{
		public static bool IsEnabled(this IFeatureManager featureManager, string feature)
		{
			return featureManager.IsEnabledAsync(feature).GetAwaiter().GetResult();
		}
	}
}
