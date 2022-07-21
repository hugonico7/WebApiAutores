namespace WebAPIAutores.Middlewares;

public static class LoguearRespuestaHttpMiddlewareExtensions
{
    public static IApplicationBuilder UseLoguearRespuestaHTTP(this IApplicationBuilder app)
    {
        return app.UseMiddleware<LoguearRespuestaHttpMiddleware>();
    }
}
public class LoguearRespuestaHttpMiddleware 
{
    private readonly RequestDelegate siguiente;
    private readonly ILogger<LoguearRespuestaHttpMiddleware> _logger;

    public LoguearRespuestaHttpMiddleware(RequestDelegate siguiente, ILogger<LoguearRespuestaHttpMiddleware> logger)
    {
        this.siguiente = siguiente;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    { 
        using (var ms = new MemoryStream())
        {
            var cuerpoOriginalRespuesta = context.Response.Body;
            context.Response.Body = ms;

            await siguiente(context);
            
            ms.Seek(0, SeekOrigin.Begin);
            var respuesta = new StreamReader(ms).ReadToEnd();
            ms.Seek(0, SeekOrigin.Begin);
            
            await ms.CopyToAsync(cuerpoOriginalRespuesta);
            context.Response.Body = cuerpoOriginalRespuesta;
            
            
            _logger.LogInformation(respuesta);

        }
        
    }
}