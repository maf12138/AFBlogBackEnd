using Microsoft.AspNetCore.Mvc.Filters;

namespace BackEndWebAPI.Attributes;

public class TransactionScopeFilter : IAsyncActionFilter
{

    public Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        throw new NotImplementedException();
    }
}
