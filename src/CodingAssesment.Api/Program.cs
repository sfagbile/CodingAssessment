using CodingAssessment.Api.Helpers;
using CodingAssessment.Api.Services;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
ExecuteApplication(builder);
// Add services to the container.
public partial class Program
{
    private static void ExecuteApplication(WebApplicationBuilder builder)
    {
        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();

        builder.Services.AddSwaggerGen(c => c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml")));

        builder.Services.AddSingleton<IPerson, Person>();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseMiddleware<ErrorHandlingMiddleware>();

        app.MapControllers();
        app.UseAuthorization();
        app.Run();
    }
}
