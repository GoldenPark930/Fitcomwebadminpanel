namespace LinksMediaCorpEntity
{
    public class LevelTeamVM
    {
        public int LevelTeamId { get; set; }
        public int TeamId { get; set; } 
        public string TeamName { get; set; }
        public  string EmailId { get; set; }
        public  string PhoneNumber { get; set; }
        public int Users { get; set; }
        public int Premium { get; set; }
        public int LevelTypeId { get; set; }
        public decimal CommissionRate { get; set; }
        public decimal Level1CommissionRate { get; set; }
        public decimal Level2CommissionRate { get; set; }
        public long TeamCommissionReportId { get; set; } 

    }
}
