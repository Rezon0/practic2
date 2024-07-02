using NetFrameworkClasses;

namespace ServerAPI
{
    public class Server
    {
        public static string url = "https://localhost:7184";
        public List<Info> infos = new List<Info>();
        public void ConfigureServices(IServiceCollection services){}

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseRouting();
        }
    }
}
