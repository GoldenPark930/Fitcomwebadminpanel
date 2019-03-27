namespace LinksMediaCorpEntity
{
    using System;
    using System.Collections.Generic;
    /// <summary>
    /// Classs for View Trainers in admin
    /// </summary>
    public partial class ViewTrainers
    {
        /// <summary> 
        /// Gets or sets the id of the trainer. 
        /// </summary> 
        public int TrainerId { get; set; }
        /// <summary> 
        /// Gets or sets the name of the trainer. 
        /// </summary> 
        public string TrainerName { get; set; }
        /// <summary> 
        /// Gets or sets the height of the trainer. 
        /// </summary> 
        public string Height { get; set; }
        /// <summary> 
        /// Gets or sets the weight of the trainer. 
        /// </summary> 
        public string Weight { get; set; }
        /// <summary> 
        /// Gets or sets the address of the trainer. 
        /// </summary> 
        public string Address { get; set; }
        /// <summary> 
        /// Gets or sets the personal information of the trainer. 
        /// </summary> 
        public string AboutMe { get; set; }
        /// <summary> 
        /// Gets or sets the three top specialization of the trainer. 
        /// </summary> 
        public string TopThreeSpecialization { get; set; }
        /// <summary> 
        /// Gets or sets the specialization list of the trainer. 
        /// </summary> 
        public List<string> SpecializationList { get; set; }
        /// <summary> 
        /// Gets or sets the team count of the trainer. 
        /// </summary> 
        public int TeamCount { get; set; }
        public string TeamName { get; set; } 
        /// <summary> 
        /// Gets or sets the profile pic of the trainer. 
        /// </summary> 
        public string TrainerImageUrl { get; set; }
        /// <summary> 
        /// Gets or sets the activity id. 
        /// </summary> 
        public string ActivityId { get; set; }
        /// <summary> 
        /// Gets or sets the flag for login. 
        /// </summary> 
        public Nullable<bool> LoggedIn { get; set; }

        public int CreatedBy { get; set; }

        public Nullable<System.DateTime> CreatedDate { get; set; }

        public int ModifyBy { get; set; }

        public Nullable<System.DateTime> ModifyDate { get; set; }

        public string TrainerType { get; set; }

        public int UniqueTeamId { get; set; }

        public int EnteredTrainerID { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public bool IsPremiumMember { get; set; }
        public string PremiumMemberStatus { get; set; }

        public bool SubscriptionStatus { get; set; }

        public int UserCredId { get; set; }

        public string SubscriptionStatusLebel { get; set; }

    }
}