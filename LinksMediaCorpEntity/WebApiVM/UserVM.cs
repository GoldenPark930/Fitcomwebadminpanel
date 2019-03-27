namespace LinksMediaCorpEntity
{
    /// <summary>
    /// Classs for Resonse of User detail
    /// </summary>
    public class UserVM
    {
        public int CredUserId { get; set; }

        public int UserId { get; set; }

        public int TeamId { get; set; } 

        public string FullName { get; set; }

        public string ImageUrl { get; set; }

        public string Address { get; set; }

        public string UserType { get; set; }

        public JoinStatus Status { get; set; }

        public string SenderEmailID { get; set; } 
    }   

}