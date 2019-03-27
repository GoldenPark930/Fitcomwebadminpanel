
namespace LinksMediaCorpEntity
{
    using System; 
    /// <summary>
    /// Classss for View Teams in admin
    /// </summary>
    public  class ViewTeams
    {
        /// <summary> 
        /// Gets or sets the id of the team. 
        /// </summary> 
        public int TeamId { get; set; } 
        /// <summary> 
        /// Gets or sets the name of the team. 
        /// </summary> 
        public string TeamName { get; set; } 
        /// <summary> 
        /// Gets or sets the address of the team. 
        /// </summary> 
        public string Address { get; set; }
        //Get or set of team email address
        public string Email { get; set; }
        /// <summary> 
        /// Gets or sets the Phone Number
        /// </summary> 
        public string PhoneNumber { get; set; }      
        /// <summary> 
        /// Gets or sets the team count of the team. 
        /// </summary> 
        public int TeamCount { get; set; }

        public int UniqueTeamId { get; set; } 
        /// <summary> 
        /// Gets or sets the profile pic of the team. 
        /// </summary> 
        public string TeamPicImageUrl { get; set; }
        /// <summary> 
        ///  Gets or sets the TeamPremium pic of the team.  
        /// </summary> 
        public string TeamPremiumImageUrl { get; set; } 
        /// <summary> 
        /// Gets or sets the flag for login. 
        /// </summary> 
        public Nullable<bool> LoggedIn { get; set; }
        public int CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public int ModifyBy { get; set; }
        public Nullable<System.DateTime> ModifyDate { get; set; }
        public bool IsDefaultTeam { get; set; }    
    }
    
}