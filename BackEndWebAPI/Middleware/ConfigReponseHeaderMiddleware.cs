namespace BackEndWebAPI.Middleware
{

    // 临时使用的一个中间件,主要是为了解决跨域问题
    public class ConfigReponseHeaderMiddleware : IMiddleware
    {
        private readonly RequestDelegate _next;

        public ConfigReponseHeaderMiddleware(RequestDelegate next)
        {
            this._next = next;
        }

        public Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            context.Response.Headers.Add("Access-Control-Allow-Origin", "*");
            return next(context);
        }
    }
}
