using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BackEndWebAPI.Filters;

public class GlobalExceptionFilter : IAsyncExceptionFilter
{
    private readonly ILogger<GlobalExceptionFilter> _logger;
    private readonly IHostEnvironment _env;

    public GlobalExceptionFilter(ILogger<GlobalExceptionFilter> logger, IHostEnvironment env)
    {
        _logger = logger;
        _env = env;
    }

    public Task OnExceptionAsync(ExceptionContext context)
    {
        var ex = context.Exception;
        _logger.LogError(ex, "未处理的异常:"+ex.Message);
        string message;
        if(_env.IsDevelopment()) message =ex.Message.ToString();
        else message = "服务器内部错误, 请联系管理人员";
        ObjectResult result = new ObjectResult(new {code=500, message=message});
        result.StatusCode = (int)System.Net.HttpStatusCode.InternalServerError;
        context.Result = result;
        context.ExceptionHandled = true;
        return Task.CompletedTask;
    }
}
