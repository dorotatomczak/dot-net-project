
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using System.Net;
using WebClinicGUI.Helpers;

namespace WebClinicGUI.Filters
{
    public class ServerConnectionExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            HttpStatusCode status = HttpStatusCode.InternalServerError;
            var exceptionType = context.Exception.GetType();
            if (exceptionType == typeof(ServerConnectionException))
            {
                context.Result = new RedirectToRouteResult(
               new RouteValueDictionary {{ "Controller", "Home" },
                                      { "Action", "ServerConnectionError" } });
            }
            else
            {
                context.Result = new RedirectToRouteResult(
            new RouteValueDictionary {{ "Controller", "Home" },
                                      { "Action", "Error" } });
            }
            context.ExceptionHandled = true;
        }
    }
}
