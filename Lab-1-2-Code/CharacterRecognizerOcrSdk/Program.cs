using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;

namespace sf.services.cognitive.vision.ocr
{
    /// <summary>
    /// This program does recognize printed text in the local and remote pictures 
    /// with .NET SDK for Computer Vision of the Cognitive Services
    /// </summary>
    class Program
    {
        // TODO: (1) IMPORTANT! Replace <API-Key> with your valid subscription key.
        const string subscriptionKey = "<API-Key>";

        // You must use the same Azure region in your REST API method as you used to
        // get your subscription keys. For example, if you got your subscription keys
        // from the East US region, replace "westcentralus" in the urlAzureRegion-Variable
        // below with "eastus".
        //
        // Free trial subscription keys are generated in the "westcentralus" region.
        // If you use a free trial subscription key, you shouldn't need to change
        // this region.
        const string urlCommon = ".api.cognitive.microsoft.com";
        // TODO: (2) assign actual azure region here:
        const string urlAzureRegion = "westcentralus";
        // const string urlAzureRegion = "eastus";
        // ... etc.
        const string urlEndpoint = "https://" + urlAzureRegion + urlCommon;

        // For printed text, change to TextRecognitionMode.Printed
        private const TextRecognitionMode textRecognitionMode = TextRecognitionMode.Printed;
        private const int numberOfCharsInOperationId = 36;


        static void Main(string[] args)
        {
            ComputerVisionClient ocrClient = new ComputerVisionClient(
                new ApiKeyServiceClientCredentials(subscriptionKey),
                new System.Net.Http.DelegatingHandler[] { });

            // Specify the Azure region
            ocrClient.Endpoint = urlEndpoint;

            // i.e. http://jeroen.github.io/images/testocr.png
            Console.Write("Enter the Web-adress of the image you wish to analyze: ");
            string urlImageFile = Console.ReadLine();
            Console.Write("Enter the local path to the image you wish to analyze: ");
            string pathImageFile = Console.ReadLine();

            Console.WriteLine("Images being analyzed ...");

#if ASYNC
        // ASYNC Run
        var t1 = RecognizeRemoteAsync(ocrClient, urlImageFile);
        var t2 = RecognizeLocalAsync(ocrClient, pathImageFile);
        Task.WhenAll(t1, t2).Wait(10000); // wait 10 sec. for the timeout
        Console.WriteLine("Press ENTER to exit");
#else
            // SYNC RUN
            AnalyzeRemoteAsync(computerVision, urlImageFile);
            AnalyzeLocalAsync(computerVision, pathImageFile);
#endif

            Console.ReadLine();
        }

        // Analyze a remote image
        private static async Task RecognizeRemoteAsync(
            ComputerVisionClient ocrClient, string imageUrl)
        {
            if (!Uri.IsWellFormedUriString(imageUrl, UriKind.Absolute))
            {
                Console.WriteLine(
                    "\nInvalid remoteImageUrl:\n{0} \n", imageUrl);
                return;
            }

            // Start the async process to recognize the text
            RecognizeTextHeaders textHeaders =
                await ocrClient.RecognizeTextAsync(
                    imageUrl, textRecognitionMode);

            await GetTextAsync(ocrClient, textHeaders.OperationLocation);
        }

        // Analyze a local image
        private static async Task RecognizeLocalAsync(
            ComputerVisionClient ocrClient, string imagePath)
        {
            if (!File.Exists(imagePath))
            {
                Console.WriteLine(
                    "\nUnable to open or read localImagePath:\n{0} \n", imagePath);
                return;
            }

            using (Stream imageStream = File.OpenRead(imagePath))
            {
                // Start the async process to recognize the text
                RecognizeTextInStreamHeaders textHeaders =
                    await ocrClient.RecognizeTextInStreamAsync(
                        imageStream, textRecognitionMode);

                await GetTextAsync(ocrClient, textHeaders.OperationLocation);
            }
        }

        // Retrieve the recognized text
        private static async Task GetTextAsync(
            ComputerVisionClient ocrClient, string operationLocation)
        {
            // Retrieve the URI where the recognized text will be
            // stored from the Operation-Location header
            string operationId = operationLocation.Substring(
                operationLocation.Length - numberOfCharsInOperationId);

            Console.WriteLine("\nCalling GetHandwritingRecognitionOperationResultAsync()");
            TextOperationResult result =
                await ocrClient.GetTextOperationResultAsync(operationId);

            // Wait for the operation to complete
            int i = 0;
            int maxRetries = 10;
            while ((result.Status == TextOperationStatusCodes.Running ||
                    result.Status == TextOperationStatusCodes.NotStarted) && i++ < maxRetries)
            {
                Console.WriteLine(
                    "Server status: {0}, waiting {1} seconds...", result.Status, i);
                await Task.Delay(1000);

                result = await ocrClient.GetTextOperationResultAsync(operationId);
            }

            // Display the results
            Console.WriteLine();
            var lines = result.RecognitionResult.Lines;
            foreach (Line line in lines)
            {
                Console.WriteLine(line.Text);
            }
            Console.WriteLine();
        }
    }
}
