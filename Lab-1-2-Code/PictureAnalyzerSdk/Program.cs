using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;

namespace sf.services.cognitive.vision
{
    class Program
    {
        // TODO: (1) IMPORTANT! Replace <API-Key> with your valid subscription key.
        const string subscriptionKey = "<API-KEY>";

        // You must use the same Azure region in your REST API method as you used to
        // get your subscription keys. For example, if you got your subscription keys
        // from the East US region, replace "westcentralus" in the URL
        // below with "eastus".
        //
        // Free trial subscription keys are generated in the "westcentralus" region.
        // If you use a free trial subscription key, you shouldn't need to change
        // this region.
        const string uriCommon = ".api.cognitive.microsoft.com";
        // TODO: (2) assign actual azure region here:
        const string uriAzureRegion = "westcentralus";
        // const string uriAzureRegion = "eastus";
        // ... etc.
        const string urlEndpoint = "https://" + uriAzureRegion + uriCommon;

      // Specify the features to return
    private static readonly List<VisualFeatureTypes> features =
        new List<VisualFeatureTypes>()
    {
            VisualFeatureTypes.Categories, VisualFeatureTypes.Description,
            VisualFeatureTypes.Faces, VisualFeatureTypes.ImageType, VisualFeatureTypes.Adult,
            VisualFeatureTypes.Tags
    };

    static void Main(string[] args)
    {
        ComputerVisionClient computerVision = new ComputerVisionClient(
            new ApiKeyServiceClientCredentials(subscriptionKey),
            new System.Net.Http.DelegatingHandler[] { });

        // Specify the Azure region
        computerVision.Endpoint = urlEndpoint;

        Console.Write("Enter the Web-adress of the image you wish to analyze: ");
        string urlImageFile = Console.ReadLine();
        Console.Write("Enter the local path to the image you wish to analyze: ");
        string pathImageFile = Console.ReadLine();
                       
        Console.WriteLine("Images being analyzed ...");

#if ASYNC
        // ASYNC Run
        var t1 = AnalyzeRemoteAsync(computerVision, urlImageFile);
        var t2 = AnalyzeLocalAsync(computerVision, pathImageFile);
        Task.WhenAll(t1, t2).Wait(10000);
        Console.WriteLine("Press ENTER to exit");
#else
        // SYNC RUN
        AnalyzeRemoteAsync(computerVision, urlImageFile);
        AnalyzeLocalAsync(computerVision, pathImageFile);
#endif

        Console.ReadLine();
    }

    // Analyze a remote image
    private static async Task AnalyzeRemoteAsync(
        ComputerVisionClient computerVision, string imageUrl)
    {
        if (!Uri.IsWellFormedUriString(imageUrl, UriKind.Absolute))
        {
            Console.WriteLine(
                "\nInvalid remoteImageUrl:\n{0} \n", imageUrl);
            return;
        }

        ImageAnalysis analysis =
            await computerVision.AnalyzeImageAsync(imageUrl, features);
        DisplayResults(analysis, imageUrl);
    }

    // Analyze a local image
    private static async Task AnalyzeLocalAsync(
        ComputerVisionClient computerVision, string imagePath)
    {
        if (!File.Exists(imagePath))
        {
            Console.WriteLine(
                "\nUnable to open or read localImagePath:\n{0} \n", imagePath);
            return;
        }

        using (Stream imageStream = File.OpenRead(imagePath))
        {
            ImageAnalysis analysis = await computerVision.AnalyzeImageInStreamAsync(
                imageStream, features);
            DisplayResults(analysis, imagePath);
        }
    }

    // Display the most relevant caption for the image
    private static void DisplayResults(ImageAnalysis analysis, string imageUri)
    {
        Console.WriteLine(String.Format("\nAnalyzed image source: {0}",imageUri));        
        Console.WriteLine(String.Format("\nImage recognition description: {0}", analysis.Description.Captions[0].Text));
        Console.WriteLine(String.Format(
                "\nIs adult content: \"{0}\". Adult-score: {1}",
                analysis.Adult.IsAdultContent, analysis.Adult.AdultScore));

        Console.WriteLine("\nCategories:\n");

       foreach (var category in analysis.Categories)
        {
                Console.WriteLine(String.Format(
                    "Category: \"{0}\". Score: {1}",
                    category.Name, category.Score));            
        }
        Console.WriteLine("\n*******************************************************\n\n");
    }
}
}
