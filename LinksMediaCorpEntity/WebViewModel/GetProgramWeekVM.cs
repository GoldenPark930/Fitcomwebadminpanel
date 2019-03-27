using System.Web.Mvc;
namespace LinksMediaCorpEntity
{
    /// <summary>
    /// Classs for Request for Get Program Week
    /// </summary>
    public class GetProgramWeekVM
    {
        public int WeekCountID { get; set; }

        public int IsNewProgramWeek { get; set; }

        public int WeekWorkoutCountID { get; set; }

        [AllowHtml]
        public string WeekItemElement { get; set; }
    }
}
