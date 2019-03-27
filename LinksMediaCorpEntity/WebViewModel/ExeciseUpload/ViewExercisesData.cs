using System.Collections.Generic;

namespace LinksMediaCorpEntity
{
    public class ViewExercisesData
    {
        public string SortField { get; set; }

        public string SortDirection { get; set; }

        public int CurrentPageIndex { get; set; }       

        public List<ViewExerciseVM> Exercises{ get; set; } 
    }
}
