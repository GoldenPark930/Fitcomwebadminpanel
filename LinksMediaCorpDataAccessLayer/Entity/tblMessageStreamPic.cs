namespace LinksMediaCorpDataAccessLayer
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class tblMessageStreamPic
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RecordId { get; set; }

        public int MessageStraemId { get; set; }

        public string PicUrl { get; set; }

        public string ImageMode { get; set; }

        public string Height { get; set; } 

        public string Width { get; set; }  
    }
}