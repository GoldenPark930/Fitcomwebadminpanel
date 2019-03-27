namespace LinksMediaCorpEntity
{
    /// <summary>
    /// Classs for Get Execise Set in admin
    /// </summary>
    public class GetExeciseSetVM
    {
        public int ExeciseCountID { get; set; }

        public int ExeciseSetCountID { get; set; }

        public string ExeciseName { get; set; }

        public int SetReps { get; set; }

        public string ResultHH { get; set; }

        public string ResultMM { get; set; }

        public string ResultSS { get; set; }

        public string ResultHS { get; set; }

        public string RestHH { get; set; }

        public string RestMM { get; set; }

        public string RestSS { get; set; }

        public string RestHS { get; set; }

        public string Execisesetdescription { get; set; }

        public string AutoCountDownYesStatus { get; set; }

        public string AutoCountDownNoStatus { get; set; }

        public string IsNewAddedSet { get; set; }

        public bool IsHideBtnDelete { get; set; }
    }
}
