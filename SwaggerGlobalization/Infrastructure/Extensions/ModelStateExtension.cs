using SwaggerGlobalization.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SwaggerGlobalization.Infrastructure.Extensions
{
    public static class ModelStateExtensions
    {
        public static string GetValidationErrorsFormatted(this ModelStateDictionary modelState)
        {
            IEnumerable<ErrorMessage> errors = GetValidationErrors(modelState);

            if (!errors.Any())
                return null;

            return string.Join("\n", errors.Where(y => !string.IsNullOrWhiteSpace(y.Message)).Select(x => x.Message).ToArray());
        }
        public static IEnumerable<ErrorMessage> GetValidationErrors(this ModelStateDictionary modelState)
        {
            var errors = new List<ErrorMessage>();
            foreach (var state in modelState)
            {
                foreach (var error in state.Value.Errors)
                {
                    errors.Add(new ErrorMessage()
                    {
                        Field = state.Key,
                        Message = error.ErrorMessage
                    });
                }
            }

            return errors;
        }
    }
}
