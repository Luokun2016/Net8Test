
using Net8Test.Utility;

using System.Text.Json.Serialization;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.AspNetCore.Server.Kestrel.Core;

namespace Net8Test
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers().AddNewtonsoftJson(option =>
            {
                option.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                option.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
                option.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;

                // Fix issue The reader's MaxDepth of 32 has been exceeded
                option.SerializerSettings.MaxDepth = 128;
            })
            .AddMvcOptions(option => option.SuppressOutputFormatterBuffering = true)
            .AddJsonOptions(option => option.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.Configure<KestrelServerOptions>(options =>
            {
                options.AllowSynchronousIO = true;
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            /*if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }*/

            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();

        }
    }
}
