using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sf.utils.text;
using System.Diagnostics;

namespace ImageLabel_CLI
{
    class Program
    {
        static void Main(string[] args)
        {
            String bucket = ExtFunc.Read("\nAmazon S3 Bucket-Name with a picture:");
            String photo = ExtFunc.Read("\nPicture Filename in Bucket:");

            // CLI-Command. Example= aws rekognition detect-labels --image "S3Object={Bucket=my-pics,Name=pic1.jpg}" 
            var cmdCli = "\n\naws rekognition detect-labels --image \"S3Object={Bucket=" + bucket + ",Name=" + photo + "}\"\n";


            ExtFunc.Say("Please run following command: " + cmdCli);
            
        }
    }
}

