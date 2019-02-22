using System;
using System.IO;
using Amazon;
using Amazon.Rekognition;
using Amazon.Rekognition.Model;
using Amazon.S3;
using Amazon.S3.Model;
using sf.utils.text;

namespace sf.ml.ImageLabel_Sdk
{
    class Program
    {
        static void Main(string[] args)
        {
            String bucket = ExtFunc.Read("\nAmazon S3 Bucket-Name with a picture:");
            String photo = ExtFunc.Read("\nPicture Filename in Bucket:");

            AmazonRekognitionClient rekognitionClient = 
                new AmazonRekognitionClient(Amazon.RegionEndpoint.USEast1);

            var analyzed_image = new Image()
            {
                S3Object = new Amazon.Rekognition.Model.S3Object()
                {
                    Name = photo,
                    Bucket = bucket
                },
            };

            DetectLabelsRequest detectlabelsRequest = new DetectLabelsRequest()
            {
                Image = analyzed_image,
                MaxLabels = 10,
                MinConfidence = 75F
            };

            try
            {
                DetectLabelsResponse detectLabelsResponse = rekognitionClient.DetectLabels(detectlabelsRequest);
                Console.WriteLine("Detected labels for " + photo);

                foreach (Label label in detectLabelsResponse.Labels)
                    Console.WriteLine("{0}: {1}", label.Name, label.Confidence);

                var tempUrl = GetS3Url(RegionEndpoint.USEast1, photo, bucket, 1);
                System.Diagnostics.Process.Start(tempUrl);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public static string GetS3Url(RegionEndpoint region,
            string filename, string bucketname, int validHours)
        {
            return new AmazonS3Client(region)
                .GetPreSignedURL(
                new GetPreSignedUrlRequest()
                {
                    BucketName = bucketname,
                    Key = filename,
                    Expires = DateTime.Now.AddHours(validHours),
                    Protocol = Protocol.HTTP
                }
                );
        }
    }
}
