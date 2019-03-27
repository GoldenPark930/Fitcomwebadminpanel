namespace LinksMediaCorpEntity
{
    /// <summary>
    /// Classs for Response for pagantion of Genric classs
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Total<T>
    {
        public T TotalList { get; set; }

        public int TotalCount { get; set; }

        public bool IsMoreAvailable { get; set; }

        public int UnReadNotificationCount { get; set; }

        public bool IsJoinedTeam { get; set; } 
    }
}