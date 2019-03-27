namespace LinksMediaCorpEntity
{
    /// <summary>
    /// Classs for Response for User Registraion
    /// </summary>
    public class UserRegistraionVM
    {
        public string TokenId { get; set; }

        public int UserId { get; set; }

        public string UserType { get; set; }

        public ProfileDetails UserDetail { get; set; }
    }
}