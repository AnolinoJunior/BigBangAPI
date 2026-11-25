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
            _logger.LogError(context.Exception, "Ocorreu uma exceção não tratada. Código de status: 500.");
            context.ExceptionHandled = true;
            context.Result = new Microsoft.AspNetCore.Mvc.ObjectResult(new { error = "Ocorreu um problema ao processar sua solicitação. Código de status: 500." })
            {
                StatusCode = 500
            };
        }
    }
