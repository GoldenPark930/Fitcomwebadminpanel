namespace LinksMediaCorpDataAccessLayer
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class tblPic
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RecordId { get; set; }

        public int UserId { get; set; }

        public string PicUrl { get; set; }

        public string Description { get; set; }
    }
}