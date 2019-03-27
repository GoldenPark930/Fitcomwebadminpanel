namespace LinksMediaCorpEntity
{
    /// <summary>
    /// Genric classs for Pagination of List response with Detail
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class NumberOfRecord<T>
    {
        public T Param { get; set; }

        public int StartIndex { get; set; }

        public int EndIndex { get; set; }
    }
    /// <summary>
    ///  Classs for Pagination of List response with Start  and Endex Index
    /// </summary>
    public class NumberOfRecord
    {
         public int StartIndex { get; set; }

         public int EndIndex { get; set; }
     }
}