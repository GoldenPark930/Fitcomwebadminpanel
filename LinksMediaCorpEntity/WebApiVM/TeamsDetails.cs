using System.Collections.Generic;
namespace LinksMediaCorpEntity
{
    /// <summary>
    /// Classs for Response of Teams Details
    /// </summary>
    public class TeamsDetails
    {
        public List<TeamsVM> Teams { get; set; }

        public bool IsJoinDefaultTeam { get; set; }

        public bool IsJoinTeam { get; set; }

        public bool isMoreFlag { get; set; }         
    }   
}