
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace LinksMediaCorpDataAccessLayer
{
   public class tblChallengeCategoryAssociation
    {
       [Key]
       [Required]
       [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
       public long ChallengeCategoryRecordId { get; set; }

       public int ChallengeId { get; set; }

       public int ChallengeCategoryId { get; set; }

       public bool IsProgram { get; set; }
    }
}
