using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NetFrameworkClasses;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WindowsService1
{
    public class MyBackgroundService : BackgroundService
    {
        private readonly ILogger<MyBackgroundService> _logger;
        private readonly HttpClient _httpClient;
        private readonly EventLog _eventLog;

        public MyBackgroundService(ILogger<MyBackgroundService> logger)
        {
            _logger = logger;
            _httpClient = new HttpClient();
            _eventLog = new EventLog();

            // Инициализация EventLog
            InitEventLog();
        }

        private void InitEventLog()
        {
            _eventLog.Source = "MySource";
            _eventLog.Log = "MyNewLog";

            if (!System.Diagnostics.EventLog.SourceExists("MySource"))
            {
                System.Diagnostics.EventLog.CreateEventSource("MySource", "MyNewLog");
            }
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    _eventLog.WriteEntry("Вход в ExecuteAsync.");

                    if (IsNetworkAvailable())
                    {
                        await TestRequests(new Info());
                    }
                    else
                    {
                        _logger.LogError("Сеть недоступна. Запрос не был выполнен.");
                        _eventLog.WriteEntry("Сеть недоступна. Запрос не был выполнен.", EventLogEntryType.Error);
                    }

                    _eventLog.WriteEntry("Завершение цикла выполнения.");

                    // Пауза перед следующей итерацией
                    await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Ошибка в цикле выполнения: {ex.Message}");
                    _eventLog.WriteEntry($"Ошибка в цикле выполнения: {ex.Message}", EventLogEntryType.Error);
                }
            }
        }

        private async Task TestRequests(Info info)
        {
            var statusMessage = new { message = "Я тут" };
            var statusContent = new StringContent(JsonConvert.SerializeObject(statusMessage), Encoding.UTF8, "application/json");

            HttpResponseMessage statusResponse = await _httpClient.PostAsync("https://localhost:7184/api/status", statusContent);
            if (statusResponse.IsSuccessStatusCode)
            {
                string responseContent = await statusResponse.Content.ReadAsStringAsync();
                _eventLog.WriteEntry($"Ответ сервера на статус: {responseContent}");
            }
            else
            {
                string errorContent = await statusResponse.Content.ReadAsStringAsync();
                _eventLog.WriteEntry($"Ответ сервера на статус: Ошибка - {statusResponse.StatusCode}, Детали: {errorContent}", EventLogEntryType.Error);
            }

            var postData = JsonConvert.SerializeObject(info);
            var content = new StringContent(postData, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PostAsync("https://localhost:7184/api/writeParameters", content);
            if (response.IsSuccessStatusCode)
            {
                string responseContent = await response.Content.ReadAsStringAsync();
                _logger.LogInformation($"Ответ сервера: {responseContent}");
            }
            else
            {
                string errorContent = await response.Content.ReadAsStringAsync();
                _logger.LogError($"Ответ сервера: Ошибка - {response.StatusCode}, Детали: {errorContent}");
                _eventLog.WriteEntry($"Ответ сервера: Ошибка - {response.StatusCode}, Детали: {errorContent}", EventLogEntryType.Error);
            }
        }

        private bool IsNetworkAvailable()
        {
            return NetworkInterface.GetIsNetworkAvailable();
        }
    }
}
