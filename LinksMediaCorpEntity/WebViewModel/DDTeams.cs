
namespace LinksMediaCorpEntity
{
    /// <summary>
    /// Classs for Get team list in view in admin
    /// </summary>
    public class DDTeams
    {
        /// <summary> 
        /// Gets or sets the id of the team. 
        /// </summary> 
        public int TeamId { get; set; }
        /// <summary> 
        /// 
        /// Gets or sets the name of the team. 
        /// </summary> 
        public string TeamName { get; set; }

        public bool IsDefaultTeam { get; set; }
    }

    public class CommisionYear
    {
        public int Year { get; set; }     
    }

    public class CommisionMonth
    {
        public int Month { get; set; }
        public string Name { get; set; }
    }
}
