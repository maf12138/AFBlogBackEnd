using BackEndWebAPI.Attributes;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Transactions;

namespace BackEndWebAPI.Filters
{
    public class UnitOfWorkFilter : IAsyncActionFilter
    {
        private static UnitOfWorkAttribute? GetUoWAttr(ActionDescriptor actionDescriptor)
        {
            var caDesc = actionDescriptor as ControllerActionDescriptor;
            if (caDesc == null)
            {
                return null;
            }
            //try to get UnitOfWorkAttribute from controller,
            //if there is no UnitOfWorkAttribute on controller, 
            //try to get UnitOfWorkAttribute from action
            var uowAttr = caDesc.ControllerTypeInfo.GetCustomAttribute<UnitOfWorkAttribute>();//通过反射获取对应控制器是否有该自定义Attribute实例
            if (uowAttr == null)
            {
                return null;
            }
            else
            {
                return caDesc.MethodInfo.GetCustomAttribute<UnitOfWorkAttribute>();
            }
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)//注意有个类似的方法,不要实现错了
        {
            var uowAttr = GetUoWAttr(context.ActionDescriptor);
            if (uowAttr == null)
            {
                await next();//传递委托(context)到下个筛选器/中间件
                return;
            }
            using TransactionScope txScope = new(TransactionScopeAsyncFlowOption.Enabled);
            List<DbContext> dbCtxs = new List<DbContext>();
            foreach (var dbCtxType in uowAttr.DbContextTypes)
            {
                //用HttpContext的RequestServices
                //确保获取的是和请求相关的Scope实例
                var sp = context.HttpContext.RequestServices;
                DbContext dbCtx = (DbContext)sp.GetRequiredService(dbCtxType);
                dbCtxs.Add(dbCtx);
            }
            var result = await next();
            if (result.Exception == null)
            {
                foreach (var dbCtx in dbCtxs)
                {
                    await dbCtx.SaveChangesAsync();
                }
                txScope.Complete();
            }
        }

    }
}
