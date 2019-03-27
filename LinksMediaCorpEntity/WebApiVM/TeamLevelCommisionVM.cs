using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinksMediaCorpEntity
{
   public class TeamLevelCommisionVM
    {
        public int PrimaryTeamId { get; set; }
        public int LevelTeamId { get; set; }
        public int LevelTypeId { get; set; } 
        public int UserOfActiveUser { get; set; }
        public int UserOfPremiumUser { get; set; }
        public long TeamCommissionReportId { get; set; }
    }
}
