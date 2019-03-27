using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LinksMediaCorpDataAccessLayer
{
    public class tblUserAcceptedProgramWeek
    {
       [Key]
       [Required]
       [DatabaseGenerated(DatabaseGeneratedOption.Identity)]   
        public long UserAcceptedProgramWeekId {get;set;}

        public int    UserAcceptedProgramId {get;set;}

        public int    WeekSequenceNumber {get;set;}

        public int    CreatedBy {get;set;}

        public DateTime? CretaedDate { get; set; }
    }
}