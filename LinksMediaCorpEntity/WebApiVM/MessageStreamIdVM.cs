namespace LinksMediaCorpEntity
{
    /// <summary>
    /// class for Request of MessageStream
    /// </summary>
    public class MessageStreamIdVM
    {
        public int MessageStreamId { get; set; }

        public int TotalPendingChallenges { get; set; }

        public bool IsAllWeekWorkoutcompleted { get; set; }

        public int UserChallengeId { get; set; }         
    }
}