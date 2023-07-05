using BackEndWebAPI.Attributes;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Reflection;
using System.Transactions;

namespace BackEndWebAPI.Filters;

public class TransactionScopeFilter : IAsyncActionFilter
{
    private readonly ILogger<TransactionScopeFilter> _logger;

    public TransactionScopeFilter(ILogger<TransactionScopeFilter> logger)
    {
        _logger = logger;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        bool hasNotTransactionScopeAttribute = false;//context.ActionDescriptor.EndpointMetadata.Any(x => x.GetType() == typeof(TransactionScopeAttribute));
        if(context.ActionDescriptor is ControllerActionDescriptor)
        {
            var actionDescriptor = (ControllerActionDescriptor)context.ActionDescriptor;
            hasNotTransactionScopeAttribute = actionDescriptor.MethodInfo.IsDefined(typeof(NotTransactionAttribute), true);//获取特性的方法很灵活
            if(hasNotTransactionScopeAttribute)
            {
                await next();
                return;
            }
            using var txScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            var result = await next();
            if (result.Exception==null)//操作方法执行没有异常
            {
                txScope.Complete();
            }
            else
            {
                _logger.LogError(result.Exception,"自动事务回滚已执行");
            }

        }
    }
}
