using Microsoft.ML.Data;

namespace npctextprediction.Model
{
    public class ModelInput
    {
        [ColumnName("text"), LoadColumn(0)]
        public string Text { get; set; }

        [ColumnName("speaker"), LoadColumn(1)]
        public string Speaker { get; set; }
    }
}