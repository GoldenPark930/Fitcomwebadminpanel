using System;
using System.Collections.Generic;
namespace LinksMediaCorpEntity
{
    public class TeamViewData
    {
        public LevelTeamVM PrimaryTeamDetail { get; set; }
        public List<LevelTeamVM> Level1TeamDetail { get; set; }
        public List<LevelTeamVM> Level2TeamDetail { get; set; }
        public int TotalLevel1Users { get; set; }
        public int TotalLevel1Premiums { get; set; }
        public int TotalLevel2Users { get; set; }
        public int TotalLevel2Premiums { get; set; }

        public decimal TotalTeamCommision { get; set; } 
        public decimal PrimaryTeamCommision { get; set; }
        public decimal Level1TeamsCommision { get; set; }
        public decimal Level2TeamsCommision { get; set; }
        public int SearchedTeamId { get; set; }
        public int SelectedYear { get; set; }
        public int SelectedMonth { get; set; }
        public List<DDTeams> LevelTeams { get; set; }     

        public DateTime? ReportDate { get; set; }
        public bool IsShowCurrentMonth { get; set; } 
        public string MonthLevel { get; set; }

        public string NoReportMessage { get; set; }

        public int PrimaryTeam { get; set; }
        public TeamViewData()
        {
           // PrimaryTeamDetail = new LevelTeamVM();
            Level1TeamDetail = new List<LevelTeamVM>();
            Level2TeamDetail = new List<LevelTeamVM>();

        }
    }

    
}
