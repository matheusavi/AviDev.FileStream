using Newtonsoft.Json;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace AviDev.FileStream.StressTest
{
    public class Stress
    {
        private static byte[] _imagemBytes;
        private static string _imagemNome;

        private readonly IHttpClientFactory _httpClientFactory;

        public Stress(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            if (_imagemBytes == null)
            {
                var fileStream = new System.IO.FileStream("./Files/thumbnail.png", FileMode.Open);
                using var memoryStream = new MemoryStream();
                fileStream.CopyTo(memoryStream);
                _imagemBytes = memoryStream.ToArray();
                _imagemNome = fileStream.Name;
            }
        }

        public async Task<int> StressImageApi()
        {
            var id = await PostImage();
            await GetImage(id);
            await DeleteImage(id);
            return id;
        }

        private async Task DeleteImage(int id)
        {
            var response =
                await _httpClientFactory
                    .CreateClient("FileStream")
                    .DeleteAsync(id.ToString());

            response.EnsureSuccessStatusCode();

            await response.Content.ReadAsStringAsync();
        }

        private async Task GetImage(int id)
        {
            var response =
                await _httpClientFactory
                    .CreateClient("FileStream")
                    .GetAsync(id.ToString());

            response.EnsureSuccessStatusCode();

            await response.Content.ReadAsStringAsync();
        }

        private async Task<int> PostImage()
        {
            MultipartFormDataContent form = new MultipartFormDataContent
            {
                { new StreamContent(new MemoryStream(_imagemBytes)), "file", _imagemNome }
            };

            var response =
                await _httpClientFactory
                    .CreateClient("FileStream")
                    .PostAsync("", form);

            response.EnsureSuccessStatusCode();
            var responseText = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ImageDto>(responseText).Id;
        }
    }
}
