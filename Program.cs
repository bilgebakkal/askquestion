using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace AskQuestion
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            Console.WriteLine("What do you want to ask me?");
            var question = Console.ReadLine();

            var answer = await CallOpenAI(250, question, "text-davinci-002", 0.7, 1, 0, 0);
            Console.WriteLine(answer);
        }

        private static async Task<string> CallOpenAI(int tokens, string input, string engine,
            double temperature, int topP, int frequencyPenalty, int presencePenalty)
        {
            var openAiKey = "Lütfen Kendi Apini Gir";
            var apiCall = $"https://api.openai.com/v1/engines/{engine}/completions";

            try
            {
                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", openAiKey);

                    var content = new
                    {
                        prompt = input,
                        temperature = temperature,
                        max_tokens = tokens,
                        top_p = topP,
                        frequency_penalty = frequencyPenalty,
                        presence_penalty = presencePenalty
                    };

                    var json = JsonConvert.SerializeObject(content);
                    var stringContent = new StringContent(json);
                    stringContent.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

                    var response = await httpClient.PostAsync(apiCall, stringContent);
                    var responseContent = await response.Content.ReadAsStringAsync();

                    dynamic dynObj = JsonConvert.DeserializeObject(responseContent);

                    if (dynObj != null)
                    {
                        return dynObj.choices[0].text.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return null;
        }
    }
}
