namespace LinksMediaCorpEntity
{
    using LinksMediaCorpUtility.Resources;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Linq;
    using System.Web;
    /// <summary>
    /// Classs for validate file size
    /// </summary>
    sealed public class ValidateFileAttribute : ValidationAttribute
    {
        /// <summary>
        /// Validate File Attribute
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public override bool IsValid(object value)
        {
            string errrormessage = string.Empty;
            int maxContent = 5 * 1024 * 1024; /*5 MB*/
            string[] sAllowedExt = new string[] { ConstantKey.JPGExetension, ConstantKey.JPEGExetension, ConstantKey.PNGExetension };
            var file = value as HttpPostedFileBase;
            if (file == null)
            {
                return true;
            }
            else if (!sAllowedExt.Contains(file.FileName.Substring(file.FileName.LastIndexOf('.')).ToUpper(CultureInfo.InvariantCulture)))
            {
                errrormessage = string.Join(", ", sAllowedExt);
                ErrorMessage = string.Format(ConstantKey.FileUploadError, errrormessage);
                // "Please upload Your Photo of type: " + ;
                return false;
            }
            else if (file.ContentLength > maxContent)
            {
                // ErrorMessage = "Your Photo is too large, maximum allowed size is : " + (maxContent / (1024 * 1024)).ToString() + "MB";
                errrormessage = (maxContent / (1024 * 1024)).ToString();
                ErrorMessage = string.Format(ConstantKey.FileUploadError, errrormessage);
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}