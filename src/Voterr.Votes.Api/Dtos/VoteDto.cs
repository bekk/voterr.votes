using System.ComponentModel.DataAnnotations;

namespace Voterr.Votes.Api.Dtos
{
    public class VoteDto
    {
        [Required]
        public int CandidateId { get; set; }
    }
}