namespace LinksMediaCorpEntity
{
    /// <summary>
    /// Classs for Get Service Response in web API
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ServiceResponse<T>
    {
        public T jsonData { get; set; }

        public bool IsResultTrue { get; set; }

        public string ErrorMessage { get; set; }

        public bool IsJoinedTeam { get; set; }  
    }

    
}