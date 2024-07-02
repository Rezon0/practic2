using NetFrameworkClasses;

namespace ServerAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var app = builder.Build();

            Server server = new Server();
            server.Configure(app, app.Environment);

            // �������� ��� ��������� ��������
            app.MapGet("/api/readParameters", () =>
            {
                if (server.infos.Count == 0) { return ""; }
                string mainStr = "";
                foreach (var item in server.infos)
                {
                    mainStr += "//////////////////////////////////////////////" + Environment.NewLine;
                    mainStr += item.ToString();
                    mainStr += @"\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\" + Environment.NewLine;
                }
                return mainStr;
            });

            app.MapPost("/api/writeParameters", async context =>
            {
                var requestBody = await context.Request.ReadFromJsonAsync<Info>();
                if (requestBody != null)
                {
                    // ���������, ��� �� ��� ����� ������ � ���������
                    if (!server.infos.Any(info => info.ToString() == requestBody.ToString()))
                    {
                        server.infos.Add(requestBody);
                        await context.Response.WriteAsJsonAsync(requestBody);
                    }
                    else
                    {
                        // ���� ������ ��� ����, ����� ������� ���������� ��� ��������� �� ������
                        context.Response.StatusCode = 400; // Bad Request
                        await context.Response.WriteAsync("������������� ������ �� �����������.");
                    }
                }
                else
                {
                    context.Response.StatusCode = 400; // Bad Request
                }
            });

            // ��������� ������� ��� /api/status
            app.MapPost("/api/status", async context =>
            {
                var statusMessage = await context.Request.ReadFromJsonAsync<StatusMessage>();
                if (statusMessage != null && !string.IsNullOrEmpty(statusMessage.Message))
                {
                    var response = new { response = "� ���" };
                    await context.Response.WriteAsJsonAsync(response);
                }
                else
                {
                    context.Response.StatusCode = 400; // Bad Request
                }
            });

            app.Run();

        }
        public class StatusMessage
        {
            public string Message { get; set; }
        }
    }
}
