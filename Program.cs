using KairoAPI.Components;
using System.Net;

namespace KairoAPI
{
    public class Program
    {   
        public static List<VersionUpdateLog> versionUpdateLogs = new List<VersionUpdateLog>();
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();

            var app = builder.Build();
            var a = File.ReadAllLines(Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"UpdateLog.txt"));
            foreach(string b in a)
            {
                var c = b.Split(";");
                List<string> list = new();
                foreach(string s in c[1].Split("/"))
                {
                    list.Add(s);
                }
                versionUpdateLogs.Add(new()
                {
                    Version = c[0],
                    UpdatedWhat = list,
                    ImportantLevel = int.Parse(c[2])
                });
            }
            app.UseRouting();
            app.MapControllers();
            app.UseMiddleware<RealIpMiddleware>();
            app.Run();
        }
        public class RealIpMiddleware
        {
            private readonly RequestDelegate _next;

            public RealIpMiddleware(RequestDelegate next)
            {
                _next = next;
            }

            public Task Invoke(HttpContext context)
            {
                var headers = context.Request.Headers;
                if (headers.ContainsKey("X-Forwarded-For"))
                {
                    context.Connection.RemoteIpAddress = IPAddress.Parse(headers["X-Forwarded-For"].ToString().Split(',', StringSplitOptions.RemoveEmptyEntries)[0]);
                }
                else if (headers.ContainsKey("X-Real-IP"))
                {
                    context.Connection.RemoteIpAddress = IPAddress.Parse(headers["X-Real-IP"].ToString());
                }
                return _next(context);
            }
        }
    }
}
