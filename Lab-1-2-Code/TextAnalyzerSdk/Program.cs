using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Threading;
using Microsoft.Rest;
using System.Net.Http;

using Microsoft.Azure.CognitiveServices.Language.TextAnalytics;
using Microsoft.Azure.CognitiveServices.Language.TextAnalytics.Models;


namespace sf.services.cognitive.text
{
    /// <summary>
    /// This program analyze the given text for language, sentiments, keyphrases and entities 
    /// with .NET SDK for Text Analyzer of the Cognitive Services
    /// </summary>
    class Program
    {
        // TODO: (1) IMPORTANT! Replace <API-Key> with your valid subscription key.
        const string subscriptionKey = "<API-Key>";

        // You must use the same Azure region in your REST API method as you used to
        // get your subscription keys. For example, if you got your subscription keys 
        // from the West Europe, replace "westcentralus" in the urlAzureRegion-Variable
        // below with "westeurope".
        //
        // Free trial subscription keys are generated in the "westcentralus" region.
        // If you use a free trial subscription key, you shouldn't need to change
        // this region.
        const string urlCommon = ".api.cognitive.microsoft.com";
        // TODO: (2) assign actual azure region here:
        const string urlAzureRegion = "westcentralus";
        // const string urlAzureRegion = "westeurope";
        // ... etc.
        const string urlEndpoint = "https://" + urlAzureRegion + urlCommon;

        class ApiKeyServiceClientCredentials : ServiceClientCredentials
        {
            public override Task ProcessHttpRequestAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                request.Headers.Add("Ocp-Apim-Subscription-Key", subscriptionKey);
                return base.ProcessHttpRequestAsync(request, cancellationToken);
            }
        }

        static void Main(string[] args)
        {
            // Create a client.
            ITextAnalyticsClient client = new TextAnalyticsClient(new ApiKeyServiceClientCredentials())
            {
                Endpoint = urlEndpoint
            };

            Console.OutputEncoding = System.Text.Encoding.UTF8;

            // Experiment 1. - analyse language
            RunExperiment1(client);

            // Experiment 2. - analyse sentiments
            RunExperiment2(client);

            // Experiment 3. - extract keyphrases
            RunExperiment3(client);

            // Experiment 4. - entities identification
            RunExperiment4(client);

            Console.ReadLine();
        }
        
        private static void RunExperiment1(ITextAnalyticsClient client)
        {
            Console.WriteLine("\n\n ===== Experiment 1. - analyse language =====");

            var input = new BatchInput(
                    new List<Input>()
                        {
                          new Input("1", "Das ist ein Text zum Testen. Sie können hier auch ihren eigenen Text in verschiedenen Sprachen hinzufügen."),
                          new Input("2", "This is a text for testing. You can also add your own text in different languages here."),
                          new Input("3", "Ceci est un texte à tester. Vous pouvez également ajouter votre propre texte dans différentes langues ici."),
                          new Input("4", "這是一個測試文本。 您也可以在此處添加不同語言的文本。")
                    });
            var result = client.DetectLanguageAsync(input).Result;

            // Printing language results.
            foreach (var document in result.Documents)
            {
                Console.WriteLine($"Document ID: {document.Id} , Language: {document.DetectedLanguages[0].Name}");
            }
        }

        private static void RunExperiment2(ITextAnalyticsClient client)
        {
            Console.WriteLine("\n\n===== Experiment 2. - analyse sentiments ======");

            SentimentBatchResult result = client.SentimentAsync(
                    new MultiLanguageBatchInput(
                        new List<MultiLanguageInput>()
                        {
                          new MultiLanguageInput("en", "0", "I had a wonderful experience! The rooms were wonderful and the staff was helpful."),
                          new MultiLanguageInput("en", "1", "I had a terrible time at the hotel. The staff was rude and the food was awful.")
                        })).Result;


            // Printing sentiment results
            foreach (var document in result.Documents)
            {
                Console.WriteLine($"Document ID: {document.Id} , Sentiment Score: {document.Score:0.00}");
            }
        }

        private static void RunExperiment3(ITextAnalyticsClient client)
        {
            Console.WriteLine("\n\n===== Experiment 3. - extract keyphrases ======");

            KeyPhraseBatchResult result = client.KeyPhrasesAsync(new MultiLanguageBatchInput(
                        new List<MultiLanguageInput>()
                        {
                          new MultiLanguageInput("ja", "1", "猫は幸せ"),
                          new MultiLanguageInput("de", "2", "Fahrt nach Stuttgart und dann zum Hotel zu Fuß."),
                          new MultiLanguageInput("en", "3", "My cat is stiff as a rock."),
                          new MultiLanguageInput("es", "4", "A mi me encanta el fútbol!")
                        })).Result;

            // Printing keyphrases
            foreach (var document in result.Documents)
            {
                Console.WriteLine($"Document ID: {document.Id} ");
                Console.WriteLine("\t Key phrases:");

                foreach (string keyphrase in document.KeyPhrases)
                {
                    Console.WriteLine($"\t\t{keyphrase}");
                }
            }
        }

        private static void RunExperiment4(ITextAnalyticsClient client)
        {
            Console.WriteLine("\n\n===== Experiment 4. - entities identification ======");

            EntitiesBatchResultV2dot1 result = client.EntitiesAsync(
                    new MultiLanguageBatchInput(
                        new List<MultiLanguageInput>()
                        {
                          new MultiLanguageInput("en", "0", "The Great Depression began in 1929. By 1933, the GDP in America fell by 25%."),
                          new MultiLanguageInput("de", "1", "Berlin is die Hauptstadt der Bundesrepublik Deutschland.")
                        })).Result;

            // Printing entities results
            foreach (var document in result.Documents)
            {
                Console.WriteLine($"Document ID: {document.Id} ");
                Console.WriteLine("\t Entities:");

                foreach (EntityRecordV2dot1 entity in document.Entities)
                {
                    Console.WriteLine($"\t\t{entity.Name}\t\t{entity.WikipediaUrl}\t\t{entity.Type}\t\t{entity.SubType}");
                }
            }
        }
    }
}
