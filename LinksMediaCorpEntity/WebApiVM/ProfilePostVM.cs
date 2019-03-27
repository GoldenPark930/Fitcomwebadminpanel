namespace LinksMediaCorpEntity
{
    /// <summary>
    /// Classs for genric for Profile post detail
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ProfilePostVM<T>
    {
        public T Stream { get; set; }

        public int UserId { get; set; }

        public string UserType { get; set; }

        public bool IsImageVideo { get; set; }
    }
}