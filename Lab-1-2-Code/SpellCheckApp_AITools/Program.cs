using Microsoft.Azure.CognitiveServices.Language.SpellCheck;
using Microsoft.Azure.CognitiveServices.Language.SpellCheck.Models;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpellCheckApp_AITools
{
    class Program
    {
        /**analyze the result*/
        static string Analyze(SpellCheckModel spellCheckModel, string text)
        {
            var tokens = spellCheckModel?.FlaggedTokens;

            if (tokens != null && tokens.Count > 0)
            {
                var sorted = tokens.ToList();

                sorted.Sort((a, b) => (a.Offset - b.Offset));

                var result = new StringBuilder();
                var pos = 0;

                foreach (var t in sorted)
                {
                    if (t.Offset > pos)
                    {
                        result.Append(text.Substring(pos, t.Offset - pos));
                        pos = t.Offset;
                    }

                    result.Append("[" + t.Suggestions.FirstOrDefault().Suggestion + "]");
                    pos += t.Token.Length;
                }

                if (text.Length > pos)
                {
                    result.Append(text.Substring(pos));
                }

                return result.ToString();
            }
            else
            {
                return text;
            }
        }

        static async Task SpellCheckAsync(string subscriptionKey)
        {
            try
            {
                Console.WriteLine("Please enter your text to correct in one line:");

                var text = Console.ReadLine();

                /**init a spellcheck client*/
                var client = new SpellCheckClient(new ApiKeyServiceClientCredentials(subscriptionKey));

                /**
                 * the API reference: https://docs.microsoft.com/zh-cn/rest/api/cognitiveservices/bing-spell-check-api-v7-reference
                 * mode: "proof", Finds most spelling and grammar mistakes
                 * market: The market must be in the form <language code>-<country code>
                 */
                var result = await client.SpellCheckerAsync(text: text, mode: "proof", market: "en-US");
                var correct = Analyze(result, text);

                Console.WriteLine("=> " + correct);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        static void Main(string[] args)
        {
            /**the spell check subscript key*/
            // TODO Replace <subscript key> with your Service-Key
            var spellCheckSubscriptionKey = "<subscript key>";

            Task.Run<Task>(async () =>
            {
                await SpellCheckAsync(spellCheckSubscriptionKey);
            }).Unwrap().GetAwaiter().GetResult();

            Console.WriteLine(Environment.NewLine + "Press ENTER to exit.");
            Console.ReadKey();
        }
    }
}
