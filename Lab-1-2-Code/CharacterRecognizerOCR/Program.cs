using System;
using System.Threading.Tasks;
using System.Text;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;

namespace sf.services.cognitive.vision.ocr
{
    /// <summary>
    /// This program does recognize printed text in the local or remote (on the Web) pictures 
    /// with REST-API for Computer Vision of the Cognitive Services
    /// </summary>
    class Program
    {
        // TODO: (1) IMPORTANT! Replace <API-Key> with your valid subscription key.
        const string subscriptionKey = "<API-Key>";

        const string urlCommon = ".api.cognitive.microsoft.com";
        const string urlVisionCommon = "/vision/v2.0/";
        const string urlVisionOcr = "ocr";

        // You must use the same Azure region in your REST API method as you used to
        // get your subscription keys. For example, if you got your subscription keys
        // from the East US region, replace "westcentralus" in the urlAzureRegion-Variable
        // below with "eastus".
        //
        // Free trial subscription keys are generated in the "westcentralus" region.
        // If you use a free trial subscription key, you shouldn't need to change
        // this region.

        // TODO: (2) assign actual azure region here:
        const string urlAzureRegion = "westcentralus";
        // const string urlAzureRegion = "eastus";
        // ... etc.

        const string urlBase = "https://" + urlAzureRegion + urlCommon + urlVisionCommon + urlVisionOcr;

        static void Main()
        {
            // Get the path and filename to process from the user.
            Console.WriteLine("Optical Character Recognition:");
            Console.Write("Enter the local path or Web-Address (URL) to an image with text you wish to read: ");
            string imageFilePath = Console.ReadLine();

            if (File.Exists(imageFilePath))
            {
                // Call the REST API method for local file.
                MakeOCRRequest(imageFilePath, false).Wait();
            }
            else if (Uri.IsWellFormedUriString(imageFilePath, UriKind.Absolute))
            {
                // Call the REST API method for local file.
                MakeOCRRequest(imageFilePath, true).Wait();
            }
            else
            {
                Console.WriteLine("\nInvalid file path or URL");
            }
            Console.WriteLine("\nPress Enter to exit...");
            Console.ReadLine();
        }

        /// <summary>
        /// Gets the text visible in the specified image file by using
        /// the Computer Vision REST API.
        /// </summary>
        /// <param name="imageFilePath">The image file with printed text.</param>
        static async Task MakeOCRRequest(string imageFilePath, bool isUrl)
        {
            try
            {
                HttpClient client = new HttpClient();

                // Request headers.
                client.DefaultRequestHeaders.Add(
                    "Ocp-Apim-Subscription-Key", subscriptionKey);

                // Request parameters. 
                // The language parameter doesn't specify a language, so the 
                // method detects it automatically.
                // The detectOrientation parameter is set to true, so the method detects and
                // and corrects text orientation before detecting text.
                string requestParameters = "language=unk&detectOrientation=true";

                // Assemble the URI for the REST API method.
                string urlRequest = urlBase + "?" + requestParameters;

                HttpResponseMessage response;

                if (isUrl)
                {   
                    var jsonString = JsonConvert.SerializeObject(new { url = imageFilePath });
                    var content = new StringContent(jsonString, Encoding.UTF8, "application/json");

                    // This example uses the "application/json" content type.
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                    Console.WriteLine("\nWait a moment for the results to appear. (Remote)\n");
                    // Asynchronously call the REST API method.
                    response = await client.PostAsync(urlRequest, content);
                }
                else
                {
                    // Read the contents of the specified local image
                    // into a byte array.
                    byte[] byteData = GetImageAsByteArray(imageFilePath);

                    // Add the byte array as an octet stream to the request body.
                    using (ByteArrayContent content = new ByteArrayContent(byteData))
                    {
                        // This example uses the "application/octet-stream" content type.
                        content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

                        Console.WriteLine("\nWait a moment for the results to appear. (Local)\n");
                        // Asynchronously call the REST API method.
                        response = await client.PostAsync(urlRequest, content);
                    }
                }

                // Asynchronously get the JSON response.
                string contentString = await response.Content.ReadAsStringAsync();

                // Display the JSON response.
                Console.WriteLine("\nResponse:\n\n{0}\n",
                    JToken.Parse(contentString).ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine("\n" + e.Message);
            }
        }

        /// <summary>
        /// Returns the contents of the specified file as a byte array.
        /// </summary>
        /// <param name="imageFilePath">The image file to read.</param>
        /// <returns>The byte array of the image data.</returns>
        static byte[] GetImageAsByteArray(string imageFilePath)
        {
            // Open a read-only file stream for the specified file.
            using (FileStream fileStream =
                new FileStream(imageFilePath, FileMode.Open, FileAccess.Read))
            {
                // Read the file's contents into a byte array.
                BinaryReader binaryReader = new BinaryReader(fileStream);
                return binaryReader.ReadBytes((int)fileStream.Length);
            }
        }
    }
}
