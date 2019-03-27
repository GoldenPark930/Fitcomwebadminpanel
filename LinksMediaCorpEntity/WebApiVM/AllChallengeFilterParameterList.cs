namespace LinksMediaCorpEntity
{
    using System.Collections.Generic;
    /// <summary>
    /// Class for All Challenge Filter Parameter List type
    /// </summary>
    /// <devdoc>
    /// Developer Name - Arvind Kumar
    /// Date - 06/09/2015
    /// </devdoc>
    public class AllChallengeFilterParameterList
    {
        public List<string> Types { get; set; }

        public List<string> Difficulties { get; set; } 

        public List<string> BodyZones { get; set; }

        public List<string> Equipments { get; set; }

        public List<string> ExerciseTypes { get; set; }
    }
}