namespace LinksMediaCorpDataAccessLayer
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class tblBoom
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BoomId { get; set; }

        public int MessageStraemId { get; set; }

        public string Message { get; set; }

        public int BoomedBy { get; set; }

        public DateTime BoomedDate { get; set; }
    }
}