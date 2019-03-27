namespace LinksMediaCorpDataAccessLayer
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class tblMessageStream
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MessageStraemId { get; set; }

        public string MessageType { get; set; }

        public string Content { get; set; }

        public int SubjectId { get; set; }

        public int TargetId { get; set; }

        public string TargetType { get; set; }

        public DateTime PostedDate { get; set; }

        public bool IsTextImageVideo { get; set; }

        public bool IsImageVideo { get; set; }

        public virtual ICollection<tblComment> Comments { get; set; }

        public bool IsNewsFeedHide { get; set; } 
        
    }
}