﻿using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwaggerGlobalization.Models.Extensions
{
    public class RangeIfAttribute : RangeAttribute
    {
        private String PropertyName { get; set; }
        private Object[] DesiredValue { get; set; }

        public RangeIfAttribute(String propertyName, Int32 minimum, Int32 maximum, params Object[] desiredvalue)
            : base(minimum, maximum)
        {
            PropertyName = propertyName;
            DesiredValue = desiredvalue;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            Object instance = validationContext.ObjectInstance;
            
            Type type = instance.GetType();
            Object proprtyvalue = type.GetProperty(PropertyName).GetValue(instance, null);

            if (DesiredValue == null) //fixes case when the only value passed that activate validation is a null value
                DesiredValue = new object[] { null };

            foreach (var d in DesiredValue)
                {
                    if ((proprtyvalue == null ? null : proprtyvalue.ToString()) == (d == null ? null : d.ToString()))
                    {
                        var localizer = validationContext.GetService(typeof(IStringLocalizer<Resources>)) as IStringLocalizer<Resources>;

                        string errorMessage = localizer == null ? null : localizer[ErrorMessageString];
                        if (!string.IsNullOrWhiteSpace(errorMessage))
                            ErrorMessage = errorMessage;

                        ErrorMessage = string.Format(ErrorMessage, validationContext.DisplayName, Minimum, Maximum);

                        ValidationResult result = base.IsValid(value, validationContext);

                        return result;
                    }
                }
            
            return ValidationResult.Success;
        }
       
    }
}
