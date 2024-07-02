using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace WindowsService1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = new HostBuilder()
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<MyBackgroundService>();
                })
                .Build();

            host.Run();
        }
    }
}
