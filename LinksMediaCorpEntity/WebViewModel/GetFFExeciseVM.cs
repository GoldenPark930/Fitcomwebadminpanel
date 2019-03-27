using System.Web.Mvc;
namespace LinksMediaCorpEntity
{
    /// <summary>
    /// Classs for Get Free Form Execise
    /// </summary>
    public class GetFFExeciseVM
    {
        public int ExeciseCountID { get; set; }

        public int ExerciseId { get; set; }

        public int Exsetcount { get; set; }

        public string IsExecisedisabled { get; set; }

        public string LinksDescription { get; set; }

        public string ExerciseThumnail { get; set; }

        public string FFExeName { get; set; }

        public string IsAlternateText { get; set; }

        public string Ischeckboxdisabled { get; set; }

        public string IsAlternateEName { get; set; }

        public string FFAExeName { get; set; }

        public string DispalyAlternateEName { get; set; }

        [AllowHtml]
        public string ExeciseItemElement { get; set; }

        public string IsNewAddedExercise { get; set; }

        public int CEARocordId { get; set; }

        public string IsExeciseCountIDText { get; set; }

    }
}
