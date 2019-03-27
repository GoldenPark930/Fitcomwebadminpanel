using System.Collections.Generic;
namespace LinksMediaCorpEntity
{
    /// <summary>
    /// classs for get Home Data for trainer in admin
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class HomeDataUpdated<T>
    {
        public ProfileDetails UserDetail { get; set; }

        public T TotalList { get; set; }

        public int TotalCount { get; set; }

        public int UserCredId { get; set; }

        public bool IsMoreAvailable { get; set; }

        public int UnReadNotificationCount { get; set; }

        public bool IsJoinedTeam { get; set; }

        public List<ChallengeCategory> PremimumTypeList { get; set; }

        public int MyAssignmentCount { get; set; }

        public UserPersoanlDetailsVM UserPersoanlDetail { get; set; }

    }
}
