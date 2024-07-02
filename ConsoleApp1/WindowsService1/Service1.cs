using NetFrameworkClasses;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace WindowsService1
{
    public partial class Service1 : ServiceBase
    {
        private EventLog eventLog1;

        public Service1()
        {
            InitializeComponent();

            eventLog1 = new System.Diagnostics.EventLog();
            if (!System.Diagnostics.EventLog.SourceExists("MySource"))
            {
                System.Diagnostics.EventLog.CreateEventSource("MySource", "MyNewLog");
            }
            eventLog1.Source = "MySource";
            eventLog1.Log = "MyNewLog";
        }

        public Service1(string[] args)
        {
            InitializeComponent();

            string eventSourceName = "MySource";
            string logName = "MyNewLog";

            if (args.Length > 0)
            {
                eventSourceName = args[0];
            }

            if (args.Length > 1)
            {
                logName = args[1];
            }

            eventLog1 = new EventLog();

            if (!EventLog.SourceExists(eventSourceName))
            {
                EventLog.CreateEventSource(eventSourceName, logName);
            }

            eventLog1.Source = eventSourceName;
            eventLog1.Log = logName;
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                eventLog1.WriteEntry("Вход в OnStart.");

                // Запуск асинхронного метода и ожидание его завершения
                TestRequests(new Info()).GetAwaiter().GetResult();

                eventLog1.WriteEntry("Служба успешно запущена.");
            }
            catch (Exception ex)
            {
                eventLog1.WriteEntry($"Ошибка при запуске службы: {ex.Message}", EventLogEntryType.Error);
                throw; // Повторно выбрасываем исключение для регистрации ошибки службы
            }
        }

        async Task TestRequests(Info info)
        {
            // Проверка наличия сети
            if (IsNetworkAvailable())
            {
                // Игнорирование SSL ошибок (не рекомендуется для продакшн среды)
                HttpClientHandler handler = new HttpClientHandler()
                {
                    ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
                };

                using (var client = new HttpClient(handler))
                {
                    try
                    {
                        // Сообщение "Я тут"
                        var statusMessage = new { message = "Я тут" };
                        var statusContent = new StringContent(JsonConvert.SerializeObject(statusMessage), Encoding.UTF8, "application/json");

                        HttpResponseMessage statusResponse = await client.PostAsync("https://localhost:7184/api/status", statusContent);
                        if (statusResponse.IsSuccessStatusCode)
                        {
                            string responseContent = await statusResponse.Content.ReadAsStringAsync();
                            eventLog1.WriteEntry($"Ответ сервера на статус: {responseContent}");
                        }
                        else
                        {
                            string errorContent = await statusResponse.Content.ReadAsStringAsync();
                            eventLog1.WriteEntry($"Ответ сервера на статус: Ошибка - {statusResponse.StatusCode}, Детали: {errorContent}", EventLogEntryType.Error);
                        }

                        // Выполнение POST запроса с передачей данных
                        var postData = JsonConvert.SerializeObject(info);
                        var content = new StringContent(postData, Encoding.UTF8, "application/json");

                        HttpResponseMessage response = await client.PostAsync("https://localhost:7184/api/writeParameters", content);
                        if (response.IsSuccessStatusCode)
                        {
                            string responseContent = await response.Content.ReadAsStringAsync();
                            Console.WriteLine($"Ответ сервера: {responseContent}");
                        }
                        else
                        {
                            string errorContent = await response.Content.ReadAsStringAsync();
                            Console.WriteLine($"Ответ сервера: Ошибка - {response.StatusCode}, Детали: {errorContent}");
                            eventLog1.WriteEntry($"Ответ сервера: Ошибка - {response.StatusCode}, Детали: {errorContent}", EventLogEntryType.Error);
                        }
                    }
                    catch (HttpRequestException httpEx)
                    {
                        Console.WriteLine($"Ошибка при выполнении HTTP-запроса: {httpEx.Message}");
                        eventLog1.WriteEntry($"Ошибка при выполнении HTTP-запроса: {httpEx.Message}", EventLogEntryType.Error);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Общая ошибка: {ex.Message}");
                        eventLog1.WriteEntry($"Общая ошибка: {ex.Message}", EventLogEntryType.Error);
                    }
                }
            }
            else
            {
                string errorMessage = "Сеть недоступна. Запрос не был выполнен.";
                Console.WriteLine(errorMessage);
                eventLog1.WriteEntry(errorMessage, EventLogEntryType.Error);
            }
        }

        protected override void OnStop()
        {
            eventLog1.WriteEntry("In OnStop.");
        }

        private bool IsNetworkAvailable()
        {
            return NetworkInterface.GetIsNetworkAvailable();
        }
    }
}
