namespace LinksMediaCorpDataAccessLayer
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class tblPieceOfEquipment
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PieceId { get; set; }

        [StringLength(20, MinimumLength = 0)]
        public string PieceName { get; set; }
    }
}