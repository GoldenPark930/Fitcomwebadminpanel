using System;
namespace LinksMediaCorpEntity
{
    /// <summary>
    /// Classs for get Sponsor Challenge Queue in admin
    /// </summary>
    public class SponsorChallengeQueue
    {
        public int QueueId { get; set; }

        public string ChallengeName { get; set; }

        public string SponsorName { get; set; }


        public int StrengthCount { get; set; }

        public string TrainerName { get; set; }

        public Nullable<System.DateTime> AcceptedDate { get; set; }
    }
}
