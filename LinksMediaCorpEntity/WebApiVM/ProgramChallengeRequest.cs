namespace LinksMediaCorpEntity
{
    /// <summary>
    /// classs for Request of Program Challenge Request
    /// </summary>
    public class ProgramChallengeRequest
    {
        public int ProgramTypeID { get; set; }

        public int ProgramCategoryID { get; set; }

        public bool IsProgramPremium { get; set; }
    }
}
