using System;

namespace Voterr.Votes.Api.Models
{
    public class Vote
    {
        public int CandidateId { get; set; }
        public string VoterObjectId { get; set; }
        public string VoterTenantId { get; set; }
    }
}