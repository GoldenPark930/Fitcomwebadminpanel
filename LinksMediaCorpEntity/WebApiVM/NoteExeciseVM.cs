using System.Collections.Generic;
namespace LinksMediaCorpEntity
{
    /// <summary>
    /// Classs for Resonse of Note Execise
    /// </summary>
    public class NoteExeciseVM
    {
        public long NotesExeciseId { get; set; }

        public string Notes { get; set; }

        public string ExeciseName { get; set; }

        public long UsedSessionNoteId { get; set; }

        public List<NoteExeciseSetVM> NoteExeciseSets { get; set; } 
    }
}