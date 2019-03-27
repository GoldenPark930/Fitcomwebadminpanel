using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinksMediaCorpDataAccessLayer
{
   public class tblTrendingCategory
    {
       [Key]
       [Required]
       [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
       public int TrendingCategoryId { get; set; } 

       public string TrendingType  { get; set; }

       public string TrendingName { get; set; }

       public string TrendingPicUrl { get; set; }

       public bool IsActive { get; set; } 
       public int TrendingCategoryGroupId { get; set; }
    }
}
