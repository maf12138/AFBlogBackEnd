using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;

namespace BackEndWebAPI.Middleware;

public class GlobalExceptionHandlingMiddleware : IMiddleware
{
    private readonly ILogger<GlobalExceptionHandlingMiddleware> _logger;
    //private readonly RequestDelegate _next;
    public GlobalExceptionHandlingMiddleware(ILogger<GlobalExceptionHandlingMiddleware> logger)
    {
        _logger = logger;
        //_next = next;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context); //调用管道执行下一个中间件
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            //按照微软的建议,再一个中间件中使用Response.xx直接修改响应状态码,响应内容,响应头等信息时,不应该再把请求传递给下一个中间件,这样会造成响应信息的不一致,因为其他的中间件可能会修改响应信息
            context.Response.StatusCode = HttpStatusCode.InternalServerError.GetHashCode();          
            ProblemDetails problemDetails= new()
            {
               Title = "服务器内部错误",
                Type = "Server Error",
                Detail = "未处理错误发生"+ e.Message,
                Status = context.Response.StatusCode,
                Instance = context.Request.Path,   
            };
            string json = JsonSerializer.Serialize(problemDetails);
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(json);
        }
    }
}
