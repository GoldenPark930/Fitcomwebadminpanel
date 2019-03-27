namespace LinksMediaCorpDataAccessLayer
{
    using System.ComponentModel.DataAnnotations;

    public class tblState
    {
        [Key]
        public string StateCode { get; set; }

        public string StateName { get; set; }
    }
}