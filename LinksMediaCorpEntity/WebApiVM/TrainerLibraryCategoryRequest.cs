namespace LinksMediaCorpEntity
{
    /// <summary>
    /// Classs for Request of TrainerLibrary Category
    /// </summary>
    public class TrainerLibraryCategoryRequest
    {
        /// <summary>
        /// User Id 
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// User Type
        /// </summary>
        public string UserType { get; set; }
        /// <summary>
        /// Challege Type Id
        /// </summary>
        public int ChallegeTypeId { get; set; }
    }
}
