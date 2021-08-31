using Newtonsoft.Json;

namespace Voterr.Votes.Api.Models
{
    public class Vote
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        
        [JsonProperty("candidateId")]
        public int CandidateId { get; set; }
        
        [JsonProperty("voterObjectId")]
        public string VoterObjectId { get; set; }
        
        [JsonProperty("voterTenantId")]
        public string VoterTenantId { get; set; }
    }
}