using CsvHelper.Configuration;
using LinksMediaCorpEntity;
namespace LinksMediaCorp
{
    public sealed class CSVExerciseMap : CsvClassMap<CSVExerciseVM>
    {
        /// <summary>
        /// Map the CSV Colum name to custom class
        /// </summary>
        public CSVExerciseMap()
        {
            Map(m => m.Name).Name("Name");
            Map(m => m.Index).Name("Index");
            Map(m => m.TeamID).Name("Team ID");
            Map(m => m.TrainerID).Name("Trainer ID");
        }
    }
    
}