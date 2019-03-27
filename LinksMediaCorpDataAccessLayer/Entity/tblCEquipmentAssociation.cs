using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LinksMediaCorpDataAccessLayer
{
    public class tblCEquipmentAssociation
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CEquipmentId { get; set; }

        public int ChallengeId { get; set; }

        public int EquipmentId { get; set; }
    }
}