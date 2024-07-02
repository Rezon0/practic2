using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace NetFrameworkClasses
{
    public class Client
    {
        static async Task Main(string[] args)
        {
            using (var client = new HttpClient())
            {
                // Выполнение GET запроса и получение данных
                HttpResponseMessage response = await client.GetAsync("/api/values");
                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Ответ сервера: {responseContent}");
                }

                // Выполнение POST запроса с передачей данных
                var postData = new StringContent("Это данные для сервера");
                response = await client.PostAsync("/api/data", postData);
                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Ответ сервера: {responseContent}");
                }
            }
        }
    }
}
