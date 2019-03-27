namespace LinksMediaCorpEntity
{
    /// <summary>
    /// Classs for Response of Session Team User detail
    /// </summary>
    public class SessionTeamUserVM
    {
        public int ID { get; set; } 

        public int CredID { get; set; }

        public int UserId { get; set; } 

        public string UserType { get; set; }

        public string FullName { get; set; }

        public string ImageUrl { get; set; }

        public int RemaingNumberOfSession { get; set; }

        public int UsedNumberOfSession { get; set; }   
    }
}