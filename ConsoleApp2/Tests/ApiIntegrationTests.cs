using NUnit.Framework;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Text;
using NetFrameworkClasses;

namespace Tests
{
    public class ApiIntegrationTests
    {
        private HttpClient _client;
        private const string BaseUrl = "https://localhost:7184";

        public ApiIntegrationTests()
        {
            _client = new HttpClient();
        }

        [Test]
        public async Task PostStatus_ReturnsSuccess()
        {
            var statusMessage = new { message = "Test message" };
            var json = JsonConvert.SerializeObject(statusMessage);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync($"{BaseUrl}/api/status", content);
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();
            Assert.IsTrue(responseString.Contains("response"));
        }

        [Test]
        public async Task PostParameters_ReturnsSuccess()
        {
            var info = new Info(); // Используйте данные для отправки
            var json = JsonConvert.SerializeObject(info);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync($"{BaseUrl}/api/writeParameters", content);
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();
            Assert.IsNotNull(responseString);
        }

        [OneTimeTearDown]
        public void Cleanup()
        {
            _client.Dispose();
        }
    }
}
