namespace LinksMediaCorpEntity
{
    /// <summary>
    /// Class for Resonse of Note Execise Set
    /// </summary>
    public class NoteExeciseSetVM
    {
        public long ExeciseSetId { get; set; }

        public int Reps { get; set; }

        public int Weight { get; set; }

        public long NotesExeciseId { get; set; }       
    }
}