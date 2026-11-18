using Microsoft.AspNetCore.Mvc.Filters;

namespace BigBangAPI.Filters;

    public class ApiExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<ApiExceptionFilter> _logger;

        public ApiExceptionFilter(ILogger<ApiExceptionFilter> logger)
        {
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            _logger.LogError(context.Exception, "Ocorreu uma exceção não tratada (HTTP 500).");
            context.ExceptionHandled = true;
            context.Result = new Microsoft.AspNetCore.Mvc.ObjectResult(new { error = "Ocorreu um erro ao processar a solicitação (HTTP 500)." })
            {
                StatusCode = 500
            };
        }
    }