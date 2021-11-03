using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using MtCoffee.Web.Models;
using System;
using System.Linq;

namespace MtCoffee.Web.Attributes
{
    public class DisableModelStateValidationAttribute : ActionFilterAttribute
    {
    }

    public class ModelStateValidationFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext actionContext)
        {
            if (actionContext.Filters.OfType<DisableModelStateValidationAttribute>().Any()) 
                return;
            // Passes validation if the param is like, Foo(Model model = null)
            // Fails validation if param is like, Foo(Model model), and the value passed in for model is null.
            // Check that any input without a default value does not fail because it lacks a required model object
            foreach (var baseParam in actionContext.ActionDescriptor.Parameters)
            {
                if (baseParam is IParameterInfoParameterDescriptor)
                {
                    var prm = (IParameterInfoParameterDescriptor) baseParam;
                    if (prm.ParameterInfo?.IsOptional == false && prm.ParameterInfo != null)
                    {
                        if (actionContext.ActionArguments.TryGetValue(prm.ParameterInfo.Name, out object val))
                        {
                            if (val == null)
                            {
                                actionContext.ModelState.AddModelError(prm.ParameterInfo.Name, string.Format("{0} is a required parameter.", prm.ParameterInfo.Name));
                            }
                        }
                        else
                        {
                            actionContext.ModelState.AddModelError(prm.ParameterInfo.Name, string.Format("{0} is a required parameter.", prm.ParameterInfo.Name));
                        }
                    }
                }
                else
                {

                }
            }

            if (!actionContext.ModelState.IsValid)
            {
                var result = new JsonPayload<object>().AddAnyErrorsFromModelState(actionContext.ModelState);
                result.StatusCode = ResponseStatusCode.INVALID_MODEL_STATE;
                actionContext.Result = result.ToJsonResult();
            }
            else
            {
                base.OnActionExecuting(actionContext);
            }
        }
    }
}
