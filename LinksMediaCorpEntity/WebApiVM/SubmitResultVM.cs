namespace LinksMediaCorpEntity
{
    /// <summary>
    /// Classs for Request of Submit Result
    /// </summary>
    public class SubmitResultVM
    {
        public int MessageStreamId { get; set; }

        public int ChallengeId { get; set; }

        public int UserId { get; set; }

        public int UserType { get; set; } 

        public string ResultValue { get; set; }

        public string FractionValue { get; set; }

        public bool IsGlobal { get; set; }

        public bool IsNewsFeed { get; set; }

        public string MessageText { get; set; }

        public bool IsTextImageVideo { get; set; }

        public bool IsImageVideo { get; set; }

        public long ProgramWeekWorkoutId {get;set;}

        public bool IsProgram { get; set; }

        public string CompletedTextMessage { get; set; } 
    }
}