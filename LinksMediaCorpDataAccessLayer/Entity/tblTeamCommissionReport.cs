using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinksMediaCorpDataAccessLayer
{
   public class tblTeamCommissionReport
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long TeamCommissionReportId { get; set; }
        public int TeamId { get; set; }
        public decimal TeamPrimaryCommissionRate { get; set; }
        public decimal Level1CommissionRate { get; set; }
        public decimal Level2CommissionRate { get; set; }
        public int ReportMonth { get; set; } 
        public int ReportYear { get; set; }
        public DateTime ReportGereratedDate { get; set; }
        public int NumberOfActiveUser { get; set; }
        public int NumberOfPremiumUser { get; set; }

    }
}
