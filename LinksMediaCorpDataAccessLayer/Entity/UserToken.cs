namespace LinksMediaCorpDataAccessLayer
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class tblUserToken
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [StringLength(50, MinimumLength = 0)]
        public string Token { get; set; }

        [StringLength(20, MinimumLength = 0)]
        public string ClientIPAddress { get; set; }

        public string TokenDevicesID { get; set; }

        public string DeviceUID { get; set; }  
         
        public string DeviceType { get; set; }  

        [Required]
        public bool IsExpired { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime ExpiredOn { get; set; }

        public bool IsRememberMe { get; set; }
    }
}