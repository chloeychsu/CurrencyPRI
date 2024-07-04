using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;

namespace CurrencyApi;

public class ActionResultFilter : ActionFilterAttribute
{
    private readonly CurrencyDBContext _context;
    private AuditTrail _audit;

    public ActionResultFilter(CurrencyDBContext context)
    {
        _context = context;
        _audit = new AuditTrail();
    }
    public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        await next();
        List<string> arguments = context.ActionArguments.Select(x => $"{x.Key} = {JsonConvert.SerializeObject(x.Value)}").ToList();
        _audit.Request = string.Join(',', arguments);
        _audit.ReqeustDateUTC = DateTime.UtcNow;
    }
    public override async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
    {
        await next();
        _audit.Action = context.ActionDescriptor.RouteValues.FirstOrDefault(x => x.Key == "action").Value;
        if (_audit.Action == "GetAllActionTrail") return;
        _audit.ClientIP = context.HttpContext.Connection?.RemoteIpAddress?.ToString();
        _audit.Controller = context.ActionDescriptor.RouteValues.FirstOrDefault(x => x.Key == "controller").Value;
        _audit.Method = context.HttpContext.Request.Method;
        _audit.IsAuthenticated = context.HttpContext.User.Identity.IsAuthenticated.ToString();
        _audit.RequestContentType = context.HttpContext.Request.ContentType;
        _audit.ResponseContentType = context.HttpContext.Response.ContentType;
        _audit.Response = ActionResult(context.Result);
        _audit.HttpStatus = context.HttpContext.Response.StatusCode;
        _audit.ModelState = context.ModelState.IsValid.ToString();
        _audit.ResponseDateUTC = DateTime.UtcNow;
        _context.AuditTrail.Add(_audit);
        await _context.SaveChangesAsync();
    }
    private string ActionResult(IActionResult actionResult)
    {
        if (actionResult is ViewResult)
        {
            var result = actionResult as ViewResult;
            return JsonConvert.SerializeObject(result.Model);
        }
        if (actionResult is ObjectResult)
        {
            var result = actionResult as ObjectResult;
            return JsonConvert.SerializeObject(result.Value);
        }
        if (actionResult is JsonResult)
        {
            var result = actionResult as JsonResult;
            return JsonConvert.SerializeObject(result.Value);
        }
        return "";
    }
}
