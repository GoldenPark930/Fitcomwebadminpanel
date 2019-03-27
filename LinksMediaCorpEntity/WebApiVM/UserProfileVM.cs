namespace LinksMediaCorpEntity
{
    /// <summary>
    /// View Model class to Serialize the User Profile data
    /// </summary>
    /// <devdoc>
    /// Developer Name - Arvind Kumar
    /// Date - 06/16/2015
    /// </devdoc>
    public class UserProfileVM
    {
        /// <summary>
        /// User Id for User/Trainer
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// User Name for User/Trainer
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// User Type of User/Trainer
        /// </summary>
        public string UserType { get; set; }
        /// <summary>
        /// Image Url Of User/Trainer
        /// </summary>
        public string ProfileImageURL { get; set; }
        /// <summary>
        /// Following Count
        /// </summary>
        public int FollowingCount { get; set; }
        /// <summary>
        /// Follower Count
        /// </summary>
        public int FollowerCount { get; set; }
        /// <summary>
        /// User Brief
        /// </summary>
        public string UserBrief { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string EmailId { get; set; }
        
        public string Zipcode { get; set; }

        public string PicBase64String { get; set; }

        public string PicName { get; set; }

        public string PicExtension { get; set; }

        public bool IsFollowByLoginUser { get; set; }

        public bool ZipcodeNotExist { get; set; }

        public int TeamId { get; set; }

        public int TeamMemberCount { get; set; }

        public bool isJoinedTeam { get; set; }

        public bool IsDefaultTeam { get; set; }
        
    }
}