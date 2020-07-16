using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SwaggerGlobalization.Models.Extensions
{
    public class ValidEnumAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {

            if (value == null)
                return ValidationResult.Success;

            var type = value.GetType();
            if (!type.IsEnum //don't activate validation if isn't enum
                || Enum.IsDefined(type, value)
                )
                return ValidationResult.Success;
            else
            {
                var localizer = validationContext.GetService(typeof(IStringLocalizer<Resources>)) as IStringLocalizer<Resources>;

                string errorMessage = localizer == null ? null : localizer[ErrorMessageString];
                if (string.IsNullOrWhiteSpace(errorMessage) || errorMessage == ErrorMessageString)
                    errorMessage = DefaultValidationMessage.ValidEnum;

                errorMessage = string.Format(errorMessage, validationContext.DisplayName);


                return new ValidationResult(errorMessage);
            }
        }
    }
}
