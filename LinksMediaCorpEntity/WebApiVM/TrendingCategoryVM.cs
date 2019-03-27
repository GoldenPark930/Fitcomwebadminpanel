namespace LinksMediaCorpEntity
{
    /// <summary>
    /// Classs for Request of Trending Category
    /// </summary>
    public class TrendingCategoryVM
    {
        public int TrendingCredId { get; set; }

        public bool IsNoTrainer { get; set; } 

        public string SelectedTeamIds { get; set; }

        public string TrendingCategoryType { get; set; }
        
    }
}
