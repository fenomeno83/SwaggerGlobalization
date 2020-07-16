using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SwaggerGlobalization.Models.Extensions
{
    public class GreaterThanAttribute : ValidationAttribute
    {

        public GreaterThanAttribute(string otherProperty, bool equalAccepted)
        {
            OtherProperty = otherProperty;
            EqualAccepted = equalAccepted;
        }

        public string OtherProperty { get; set; }
        public bool EqualAccepted { get; set; }

        public string FormatErrorMessage(string name, string otherName, IStringLocalizer<Resources> localizer)
        {

            string errorMessage = localizer == null ? null : localizer[ErrorMessageString];
            if (string.IsNullOrWhiteSpace(errorMessage) || errorMessage == ErrorMessageString)
                errorMessage = EqualAccepted ? DefaultValidationMessage.GreatherEqualsThan : DefaultValidationMessage.GreatherThan;

            return string.Format(errorMessage, name, otherName);
        }

        protected override ValidationResult
            IsValid(object firstValue, ValidationContext validationContext)
        {
            var firstComparable = firstValue as IComparable;
            var secondComparable = GetSecondComparable(validationContext);


            if (firstComparable != null && secondComparable != null)
            {
                if ((!EqualAccepted ? true : firstComparable.ToString() != secondComparable.ToString()) && firstComparable.CompareTo(secondComparable) < 1)
                {
                    object obj = validationContext.ObjectInstance;

                    var localizer = validationContext.GetService(typeof(IStringLocalizer<Resources>)) as IStringLocalizer<Resources>;

                    return new ValidationResult(
                        FormatErrorMessage(validationContext.DisplayName, OtherProperty, localizer));
                }
            }

            return ValidationResult.Success;
        }

        protected IComparable GetSecondComparable(
            ValidationContext validationContext)
        {
            var propertyInfo = validationContext
                                  .ObjectType
                                  .GetProperty(OtherProperty);
            if (propertyInfo != null)
            {
                var secondValue = propertyInfo.GetValue(
                    validationContext.ObjectInstance, null);
                return secondValue as IComparable;
            }
            return null;
        }
    }

}
