
using Microsoft.EntityFrameworkCore;
using WebApi.Context;
using WebApi.Middlewares;
using System.Reflection;
using System.Text.Json.Serialization;
using WebApi.Services;

namespace WebApi;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddDbContext<FinancesContext>(f =>
            f.UseNpgsql(builder.Configuration["ConnStr"])
            .UseSnakeCaseNamingConvention());


        builder.Services.AddAutoMapper(Assembly.GetAssembly(typeof(Program)));

        builder.Services.AddScoped<ICategoriesService, CategoriesService>();
        builder.Services.AddScoped<IPeriodsService, PeriodsService>();
        builder.Services.AddScoped<ITransactionService, TransactionService>();
        builder.Services.AddScoped<IFinancialGoalsService, FinancialGoalsService>();

        builder.Services.AddCors(options =>
        {
            options.AddPolicy("cors", policy =>
            {
                policy.AllowAnyHeader()
                .AllowAnyMethod()
                .AllowAnyOrigin();
            });
        });

        builder.Services.AddControllers().AddJsonOptions(opt =>
        {
            opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        });

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();

        builder.Services.AddSwaggerGen();


        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseMiddleware<StatusCodeMiddleware>();

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.UseCors("cors");

        app.MapControllers();

        app.Run();
    }
}
