using Microsoft.ML;
using Microsoft.ML.Data;
using npctextprediction.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace npctextprediction
{
    public class ConsumeModel
    {
        public static List<KeyValuePair<string, decimal>> Predict(ModelInput input, string modelPath)
        {
            var mlContext = new MLContext();

            var mlModel = mlContext.Model.Load(modelPath, out var modelInputSchema);
            var predEngine = mlContext.Model.CreatePredictionEngine<ModelInput, ModelOutput>(mlModel);
            var result = predEngine.Predict(input);

            var labelBuffer = new VBuffer<ReadOnlyMemory<char>>();
            predEngine.OutputSchema["Score"].Annotations.GetValue("SlotNames", ref labelBuffer);
            var labels = labelBuffer.DenseValues().Select(l => l.ToString()).ToArray();

            var topScores = labels.ToDictionary(
                l => l,
                l => (decimal)result.Score[Array.IndexOf(labels, l)] * 100
            )
            .OrderByDescending(kv => kv.Value)
            .Take(5).ToList();

            return topScores;
        }
    }
}