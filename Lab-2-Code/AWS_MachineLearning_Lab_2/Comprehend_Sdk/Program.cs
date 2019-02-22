using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Amazon.Comprehend;
using Amazon.Comprehend.Model;
using sf.utils.text;


namespace Comprehend_Sdk
{
    class Program
    {
        static void Main(string[] args)
        {
            String text = ExtFunc.Read("Input a text to Sentiment-Analysation i.E \"That was a nice trip. The Hotel was amazing!\"");

            AmazonComprehendClient comprehendClient = new AmazonComprehendClient(
                Amazon.RegionEndpoint.USEast1);

            // Call DetectKeyPhrases API
            Console.WriteLine("Calling DetectSentiment");
            DetectSentimentRequest detectSentimentRequest = new DetectSentimentRequest()
            {
                Text = text,
                LanguageCode = "en"
            };
            DetectSentimentResponse detectSentimentResponse = comprehendClient.DetectSentiment(detectSentimentRequest);
            Console.WriteLine(detectSentimentResponse.Sentiment);
            Console.WriteLine("Done");
        }
    }
}

