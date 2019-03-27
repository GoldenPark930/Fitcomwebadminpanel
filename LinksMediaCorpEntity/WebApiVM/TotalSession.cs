namespace LinksMediaCorpEntity
{
    /// <summary>
    /// Classs for Total sesssion Genric classs for user/trainer session
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class TotalSession<T>
    {
        public T TotalList { get; set; }

        public int TotalCount { get; set; }

        public bool IsMoreAvailable { get; set; }

        public int UserCredID { get; set; } 

        public int RemaingNumberOfSession { get; set; } 
    }
   
}