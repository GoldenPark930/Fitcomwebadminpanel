
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace LinksMediaCorpDataAccessLayer
{
   public class tblTeamTrendingAssociation
    {
       [Key]
       [Required]
       [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
       public int TreamTrendingId { get; set; }

       public int TeamId { get; set; }

       public string TrendingCategoryType { get; set; } 

       public int TrendingCategoryId { get; set; } 
    } 
}
