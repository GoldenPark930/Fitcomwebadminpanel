namespace LinksMediaCorpDataAccessLayer
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    /// <summary>
    /// Classs for get UserToken in web api
    /// </summary>
    public class UserToken
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [StringLength(50, MinimumLength = 0)]
        public string Token { get; set; }

        [Required]
        public bool IsExpired { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime ExpiredOn { get; set; }
    }
}