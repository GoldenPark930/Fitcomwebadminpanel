namespace LinksMediaCorpEntity
{
    using System.ComponentModel.DataAnnotations;
    /// <summary>
    /// Class for Edit Password
    /// </summary>
    public class EditPasswordVM
    {
        [Required]
        [MaxLength(20, ErrorMessage = "Please enter max 20 character in passwod")]
        public string OldPassword {get;set;}

        [Required]
        [MaxLength(20, ErrorMessage = "Please enter max 20 character in passwod")]
        public string NewPassword { get; set; }
    }
}