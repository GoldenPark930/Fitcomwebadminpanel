namespace LinksMediaCorpEntity
{
    /// <summary>
    /// Classs for Mesage stream with Generic class
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PostVM<T>
    {
        public T Stream { get; set; }

        public int TargetId { get; set; }

        public bool IsImageVideo { get; set; }
    }
}