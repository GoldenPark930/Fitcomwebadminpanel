namespace LinksMediaCorpEntity
{
    using System.Collections.Generic;
    /// <summary>
    /// Classs for Resonse for Team Detail
    /// </summary>
    public class TeamVM
    {
        public int CredUserId { get; set; }

        public int TeamId { get; set; }

        public int TeamCredId { get; set; }

        public bool IsJoinByUniqueTeamId { get; set; }  
    }
}