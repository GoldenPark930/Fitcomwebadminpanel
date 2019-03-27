using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LinksMediaCorpDataAccessLayer
{
   public class tblLevelTeam
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TeamLevelId { get; set; }
        public string GuidRecordId { get; set; }
        public int PrimaryTeamId { get; set; }
        public int LevelTeamId { get; set; }
        public int LevelTypeId { get; set; }    
        public Nullable<DateTime> CreatedDate { get; set; }
        public Nullable<DateTime> ModifiedDate { get; set; }
        public bool IsActive { get; set; }
        

    }
}
