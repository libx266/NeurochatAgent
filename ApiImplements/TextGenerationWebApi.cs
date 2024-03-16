using ApiInterface;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace ApiImplements
{
    public sealed class TextGenerationWebApi : INeuroApi, IDisposable
    {
        private readonly HttpClient _http;

        public TextGenerationWebApi(float temperature, float repetitionPenalty, float topP, string ip = "127.0.0.1", ushort port = 5000) : base(temperature, repetitionPenalty, topP)
        {
            _http = new HttpClient();
            _http.BaseAddress = new Uri($"http://{ip}:{port}/v1/");
            _http.Timeout = TimeSpan.FromMinutes(2);
        }

        private static StringContent MakeJson(object obj) => new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json");

        public override async Task<string?> GenerateBase(string prompt, ushort maxTokens, string stopWord)
        {
            var data = new
            {
                prompt = prompt,
                max_tokens = maxTokens,
                temperature = _temperature,
                top_p = _topP,
                repetition_penalty = _repetitionPenalty,
                stop = new[] { stopWord }
            };

            var data2 = MakeJson(data);

            try
            {
                var response = await _http.PostAsync("completions", data2);
                var answer = JsonConvert.DeserializeObject<dynamic>(await response.Content.ReadAsStringAsync());

                return answer?.choices[0].text;
            }
            catch (Exception ex) 
            {
                Console.WriteLine("Generation timeout");
                return default;
            }
        }

        public void Dispose() => _http.Dispose();
    }
}
