
namespace LinksMediaCorpEntity
{
    /// <summary>
    /// Class for Resonse of Main Search on fitcom app
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SearchResponse<T>
    {
        public T jsonData { get; set; } 

        public bool IsResultTrue { get; set; }

        public string ErrorMessage { get; set; }

        public int TotalCount { get; set; }

        public bool IsMoreAvailable { get; set; } 
    }

   
}