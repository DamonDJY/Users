using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Users.WebApi
{
    public class UnitOfWorkFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var uowAttr = GetUowAttr(context.ActionDescriptor);
            if (uowAttr == null)
            {
                await next();
                return;
            }
            List<DbContext> dbContexts= new List<DbContext>();

            foreach(var dbCtxType in uowAttr.DbContextTypes)
            {
                var sp = context.HttpContext.RequestServices;
                DbContext dbCtx =(DbContext)sp.GetRequiredService(dbCtxType);
                dbContexts.Add(dbCtx);
            }
            var result = await next();

            if(result.Exception ==null)
            {
                foreach(var dbCtx in dbContexts)
                {
                    await dbCtx.SaveChangesAsync();
                }
            }
        }

        private static UnitOfWorkAttribute? GetUowAttr(ActionDescriptor actionDescriptor)
        {
            var caDesc = actionDescriptor as ControllerActionDescriptor;
            if(caDesc  == null)
            {
                return null;
            }

            var uowAttr = caDesc.ControllerTypeInfo.GetCustomAttribute<UnitOfWorkAttribute>();

            if(uowAttr != null)
            {
                return uowAttr;
            }
            else
            {
                return caDesc.MethodInfo.GetCustomAttribute<UnitOfWorkAttribute>();
            }
        }
    }
}
