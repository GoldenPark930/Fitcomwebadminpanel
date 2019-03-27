namespace LinksMediaCorpDataAccessLayer
{
    using System;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

    public class tblComment
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CommentId { get; set; }

        public int MessageStraemId { get; set; }

        public string Message { get; set; }

        public int CommentedBy { get; set; }

        public DateTime CommentedDate { get; set; }

        public virtual tblMessageStream MessageStream { get; set; }
    }
}