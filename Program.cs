using GUI_2.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace GUI_2
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            //builder.Services.AddCors(o =>                   //Registrierung der Blazor UI
            //{
            //    o.AddPolicy("blazor", p => p
            //        .WithOrigins("https://localhost:7066") //Blazor URL
            //        .AllowAnyHeader()
            //        .AllowAnyMethod()
            //        .AllowCredentials());
            //});


            // Add services to the container.
            builder.Services.AddHttpContextAccessor();             //Accessor der meinem Controller Zugriff auf den HttpContext erlaubt um Daten abzugreifen und automatisch reinzuschreiben
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddControllers()
                .AddJsonOptions(o => o.JsonSerializerOptions.PropertyNamingPolicy = null);

            // Swagger/OpenAPI Setup
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
                //app.UseStaticFiles();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.UseMiddleware<UserContextMiddleware>();

            app.Run();
        }
    }
}