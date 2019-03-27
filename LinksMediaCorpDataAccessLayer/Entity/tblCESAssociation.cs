using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace LinksMediaCorpDataAccessLayer
{
    public class tblCESAssociation
    {

        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long  ExeciseSetId { get; set; }

        public bool IsRestType { get; set; }

        public string SetResult { get; set; }

        public string RestTime { get; set; }

        public int SetReps { get; set; }

        public string Description { get; set; }
        public int RecordId { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool AutoCountDown { get; set; }
    }
}