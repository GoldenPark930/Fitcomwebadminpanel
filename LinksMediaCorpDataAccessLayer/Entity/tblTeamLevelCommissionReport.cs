using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinksMediaCorpDataAccessLayer
{
   public  class tblTeamLevelCommissionReport
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long TeamLevelCommissionReportId { get; set; }        
        public int PrimaryTeamId { get; set; }
        public int LevelTeamId { get; set; }
        public int LevelTypeId { get; set; }
        public int NumberOfActiveUser { get; set; }
        public int NumberOfPremiumUser { get; set; }
        public long TeamCommissionReportId { get; set; }
    }
}
