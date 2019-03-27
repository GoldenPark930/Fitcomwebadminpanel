
namespace LinksMediaCorpEntity
{
    /// <summary>
    /// Classs for get Execise Set details in admin
    /// </summary>
    public class ExeciseSetVM
    {
        public bool IsRestType { get; set; }

        public string SetResult { get; set; }

        public string RestTime { get; set; }

        public int SetReps { get; set; }

        public string Description { get; set; }

        public string AutoCountDown { get; set; }

        public bool IsAutoCountDown { get; set; }   
    }
}