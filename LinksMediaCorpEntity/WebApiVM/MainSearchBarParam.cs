
namespace LinksMediaCorpEntity
{
   /// <summary>
    /// Class for Request for Main SearchBarParam
   /// </summary>
    public class MainSearchBarParam
    {
        public string SearchString { get; set; }

        public TabEnum SelectTab { get; set; }

        public int StartIndex { get; set; }

        public int EndIndex { get; set; }
    }
}
