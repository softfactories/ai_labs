using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sf.utils.text;

namespace Comprehend_CLO
{
    class Program
    {
        static void Main(string[] args)
        {
            // CLI-Command. 
            var cmdCli = "\n\naws comprehend detect-sentiment --region \"us-east-1\" " +
                "--language-code \"en\" --text \"That was a nice trip. The Hotel was amazing!\"\n";

            /*That was a nice trip. The Hotel was amazing!*/

            ExtFunc.Say("Please run following command: " + cmdCli);
        }
    }
}
