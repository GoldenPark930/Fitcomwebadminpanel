using System;
using System.Collections.Generic;
namespace LinksMediaCorpEntity
{
    /// <summary>
    /// Classs for Resonse of Notes User Session
    /// </summary>
    public class NotesUsedSessionVM
    {
        public long UsedSessionNoteId { get; set; }    

        public long UseSessionId { get; set; }

        public bool IsTracNotes { get; set; }

        public string Notes { get; set; }       
            
        public int UserId { get; set; }

        public int UserType { get; set; } 

        public int CreatedBy { get; set; }

        public List<NoteExeciseVM> NoteExecises { get; set; }    
          
        public Nullable<System.DateTime> CreatedDatetime { get; set; }

        public int ModifiedBy { get; set; }

        public Nullable<System.DateTime> ModifiedDatetime { get; set; }
    }  
}