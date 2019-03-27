namespace LinksMediaCorpEntity
{
    using System;
    /// <summary>
    /// Classs for grt user result in admin
    /// </summary>
    public class UserResult
    {
        public int Id { get; set; }
        public int ChallengeId { get; set; }
        public int UserId { get; set; }
        public DateTime AcceptedDate { get; set; }
        public string Result { get; set; }
        public string Fraction { get; set; }
        public int CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public int ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
    }
}