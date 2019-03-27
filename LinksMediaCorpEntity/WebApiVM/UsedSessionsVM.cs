using System;
using System.ComponentModel.DataAnnotations;
namespace LinksMediaCorpEntity
{
    /// <summary>
    /// Classs for Response of Used Sessions
    /// </summary>
    public class UsedSessionsVM
    {
        public long UseSessionId { get; set; }

        public long UseSessionNoteId { get; set; } 

        [Required]
        public string UseSessionDateTime { get; set; }  
               
        public bool IsAttended { get; set; }

        public bool IsTracNotes { get; set; }

        public int UserCredId { get; set; }

        public int TrainerId { get; set; }

        public UserTrainingType TrainingType { get; set; }

        public int CreatedBy { get; set; }

        public int UserId { get; set; } 

        public string UserType { get; set; }     
          
        public Nullable<System.DateTime> CreatedDatetime { get; set; }

        public int ModifiedBy { get; set; }

        public Nullable<System.DateTime> ModifiedDatetime { get; set; }

        public string  Notes { get; set; }  
    }
}