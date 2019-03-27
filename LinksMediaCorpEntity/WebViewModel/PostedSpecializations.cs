using System.Collections.Generic;
namespace LinksMediaCorpEntity
{
   /// <summary>
    /// classs for add Posted Specializations in admin
   /// </summary>
    public class PostedSpecializations
    {
        public List<string> SpecializationIDs { get; set; }

        public List<string> PrimarySpecializationIDs { get; set; }

        public List<string> SecondarySpecializationIDs { get; set; }
    }
   
}