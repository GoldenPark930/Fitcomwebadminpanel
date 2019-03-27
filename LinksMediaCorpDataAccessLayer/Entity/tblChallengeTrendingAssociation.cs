
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace LinksMediaCorpDataAccessLayer
{
   public class tblChallengeTrendingAssociation
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ChallengeTrendingId { get; set; } 

        public int ChallengeId { get; set; }

        public int TrendingCategoryId { get; set; }

        public bool IsProgram { get; set; }
    }
}
