using Microsoft.AspNetCore.Mvc.Filters;

namespace BigBangAPI.Filters;

public class ApiLoggingFilter : IActionFilter
{
    private readonly ILogger<ApiLoggingFilter> _logger;

    public ApiLoggingFilter(ILogger<ApiLoggingFilter> logger)
    {
        _logger = logger;
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
          _logger.LogInformation("### Executando -> início da ação");
    _logger.LogInformation("############################################");
    _logger.LogInformation($"{DateTime.Now.ToLongTimeString()}");
    _logger.LogInformation($"Estado de validação do modelo: {context.ModelState.IsValid}");
    _logger.LogInformation("############################################");
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        _logger.LogInformation("### Executando -> fim da ação");
    _logger.LogInformation("############################################");
    _logger.LogInformation($"{DateTime.Now.ToLongTimeString()}");
    _logger.LogInformation($"Código de status: {context.HttpContext.Response.StatusCode}");
    _logger.LogInformation("############################################");
    }
}
