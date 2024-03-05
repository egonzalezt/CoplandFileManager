namespace CoplandFileManager.Extensions;

using Microsoft.OpenApi.Models;
using System.Reflection;

public static class SwaggerConfiguration
{
    public static WebApplication AddSwaggerUi(this WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.InjectStylesheet("/swagger-ui/SwaggerDark.css");
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
            options.DefaultModelsExpandDepth(-1);
            options.DisplayRequestDuration();
            options.EnableDeepLinking();
            options.EnableFilter();
        });
        return app;
    }

    public static void ConfigureSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new()
            {
                Version = "v1",
                Title = "Copland File Manager",
                Description = "A futuristic file management microservice inspired by the anime Serial Experiments Lain. " +
                "Copland File Manager empowers users to seamlessly upload, organize, and secure their digital assets within " +
                "a dynamic and cybernetic environment. Embrace the essence of Lain as you navigate the digital labyrinth and " +
                "transcend the boundaries of conventional file management.",

                License = new OpenApiLicense
                {
                    Name = "Apache 2.0",
                    Url = new Uri("https://www.apache.org/licenses/LICENSE-2.0"),
                }
            });
            var xmlFileName = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFileName));
        });
    }
}
