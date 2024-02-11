using Microsoft.AspNetCore.Mvc.Filters;

namespace Chamedoon.WebAPI.Models
{

    public class LoggingAttribute : Attribute
    {
        private readonly ILogger<LoggingAttribute> _logger;

        public LoggingAttribute(ILogger<LoggingAttribute> logger)
        {
            _logger = logger;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            _logger.LogInformation("Action method is about to be executed.");
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            _logger.LogInformation("Executing action: {Action} ({ActionType})", context.ActionDescriptor.DisplayName, context.ActionDescriptor.DisplayName);

            _logger.LogInformation("Action method has been executed.");
        }
    }
    public enum ActionType
    {
        MyAction,
        YourAction,
        AnotherAction
    }

    public class ActionLoggingAttribute : ActionFilterAttribute
    {
        private readonly ILogger<ActionLoggingAttribute> _logger;

        public ActionLoggingAttribute(ILogger<ActionLoggingAttribute> logger)
        {
            _logger = logger;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            _logger.LogInformation("Executing action: {Action} ({ActionType})", context.ActionDescriptor.DisplayName, context.ActionDescriptor.DisplayName);
        }
    }


}
