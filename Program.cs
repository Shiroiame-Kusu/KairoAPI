using KairoAPI.Components;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace KairoAPI
{
    public class Program
    {
        public static List<VersionUpdateLog> versionUpdateLogs = new List<VersionUpdateLog>();
        public static JsonResult VULJsonResult { get; set; }
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            });

            builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            var app = builder.Build();
            var a = File.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "UpdateLog.txt")) ? File.ReadAllLines(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "UpdateLog.txt")) : throw new FileNotFoundException();
            foreach (string b in a)
            {
                var c = b.Split(";");
                List<string> list = new();
                foreach (string s in c[1].Split("/"))
                {
                    list.Add(s);
                }
                var d = c[0].Split("-");
                var e = d[1].Split(".");
                versionUpdateLogs.Add(new()
                {
                    Version = d[0],
                    Channel = e[0],
                    VersionCode = CodeSwitch(d[0]),
                    Subversion = int.Parse(e[1]),
                    UpdatedWhat = list,
                    ImportantLevel = int.Parse(c[2])
                });
            }
            VULJsonResult = new JsonResult(versionUpdateLogs);
            app.UseRouting();
            app.MapControllers();
            app.UseMiddleware<RealIpMiddleware>();
            app.Run();
        }
        private static string CodeSwitch(string a)
        {
            if (a == "2.4.0")
            {
                return "Crychic";
            }
            return null;
        }
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
