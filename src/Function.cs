using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Converters;
using npctextprediction.Model;
using System;
using System.Net;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace npctextprediction
{
    public static class Function
    {
        [FunctionName(nameof(PredictBG))]
        [OpenApiOperation(operationId: nameof(PredictBG), tags: new[] { "text" })]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
        [OpenApiParameter(name: "text", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "The text to process")]
        [OpenApiParameter(name: "modelType", In = ParameterLocation.Query, Required = true, Type = typeof(ModelType), Description = "The model type to use")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "The OK response")]
        public static async Task<IActionResult> PredictBG(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
            ILogger log,
            ExecutionContext context)
        {
            log.LogInformation($"{nameof(PredictBG)} called");

            string text = req.Query["text"];
            string modelType = req.Query["modelType"];
            if (!String.IsNullOrEmpty(text) && (Enum.TryParse(typeof(ModelType), modelType, out var model)))
            {
                var sampleData = new ModelInput()
                {
                    Text = text
                };

                var modelPath = "";
                switch (model)
                {
                    case ModelType.BG:
                        modelPath = context.FunctionAppDirectory + "./MLModel/BGMLModel.zip";
                        break;
                    case ModelType.BG1:
                        modelPath = context.FunctionAppDirectory + "./MLModel/BG1MLModel.zip";
                        break;
                    case ModelType.BG2:
                        modelPath = context.FunctionAppDirectory + "./MLModel/BG2MLModel.zip";
                        break;
                }

                log.LogInformation($"Model set as {modelPath}");

                if (!String.IsNullOrEmpty(modelPath))
                {
                    var predictionResult = ConsumeModel.Predict(sampleData, modelPath);

                    return new OkObjectResult(predictionResult);
                }

                return new OkObjectResult("Unknown model type");
            }

            return new OkObjectResult("Missing text or invalid model type");
        }

        [JsonConverter(typeof(StringEnumConverter))]
        public enum ModelType
        {
            [EnumMember(Value = "BG")]
            BG,
            [EnumMember(Value = "BG1")]
            BG1,
            [EnumMember(Value = "BG2")]
            BG2
        }
    }
}