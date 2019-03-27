namespace LinksMediaCorpEntity
{
    /// <summary>
    /// Classs for Resonse of Follow User
    /// </summary>
    public class FollowUserVM
    {
        public int UserId { get; set; }

        public string UserType { get; set; }

        public bool IsFollow { get; set; }      
    }
}