using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using SwaggerGlobalization.Interfaces;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SwaggerGlobalization.Infrastructure.Extensions
{
    public class CustomRequestBodyFilter : IRequestBodyFilter
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IStringLocalizer _localizer;


        public CustomRequestBodyFilter(IConfiguration configuration, IHttpContextAccessor httpContextAccessor, IStringLocalizer<Resources> localizer)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _localizer = localizer;

        }

        public void Apply(OpenApiRequestBody requestBody, RequestBodyFilterContext context)
        {
            string queryLang = _httpContextAccessor?.HttpContext.Request?.Query["lang"].FirstOrDefault();
            if (!string.IsNullOrWhiteSpace(queryLang))
                Thread.CurrentThread.SetCulture(queryLang);
            else
            {
                string culture = _configuration["Globalization:DefaultCulture"];
                Thread.CurrentThread.SetCulture(culture);

            }

            var descr = requestBody.Description.RemoveNewLineAndTrim();
            if (!string.IsNullOrWhiteSpace(descr))
            {
                var locDescr = _localizer[descr];
                if (locDescr != descr)
                    requestBody.Description = locDescr;
            }

        }
    }

    public class CustomParameterFilter : IParameterFilter
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IStringLocalizer _localizer;


        public CustomParameterFilter(IConfiguration configuration, IHttpContextAccessor httpContextAccessor, IStringLocalizer<Resources> localizer)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _localizer = localizer;

        }

        public void Apply(OpenApiParameter parameter, ParameterFilterContext context)
        {
            string queryLang = _httpContextAccessor?.HttpContext.Request?.Query["lang"].FirstOrDefault();
            if (!string.IsNullOrWhiteSpace(queryLang))
                Thread.CurrentThread.SetCulture(queryLang);
            else
            {
                string culture = _configuration["Globalization:DefaultCulture"];
                Thread.CurrentThread.SetCulture(culture);

            }

            var descr = parameter.Description.RemoveNewLineAndTrim();
            if (!string.IsNullOrWhiteSpace(descr))
            {
                var locDescr = _localizer[descr];
                if (locDescr != descr)
                    parameter.Description = locDescr;
            }
        }
    }
    public class CustomOperationFilter : IOperationFilter
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IStringLocalizer _localizer;
        const string captureName = "routeParameter";


        public CustomOperationFilter(IConfiguration configuration, IHttpContextAccessor httpContextAccessor, IStringLocalizer<Resources> localizer)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _localizer = localizer;

        }

        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {

            var list = _configuration.GetSection("Globalization:SupportedCultures").Get<List<string>>().Where(x => !string.IsNullOrWhiteSpace(x)).Distinct().ToList();
            bool.TryParse(_configuration["Globalization:Swagger:EnableMultilanguageDocumentation"], out bool enableMultiLangSwagger);
            bool.TryParse(_configuration["Globalization:Swagger:EnableLocalizedDocumentation"], out bool enableLocalizedDoc);
            bool.TryParse(_configuration["Globalization:Swagger:EnableAcceptLanguageParam"], out bool enablesAccLang);
            string defaultCulture = null;

            if (enableMultiLangSwagger && list != null && list.Count > 1 && enableLocalizedDoc)
            {
                string queryLang = _httpContextAccessor?.HttpContext.Request?.Query["lang"].FirstOrDefault();
                if (!string.IsNullOrWhiteSpace(queryLang))
                {
                    defaultCulture = queryLang;
                    Thread.CurrentThread.SetCulture(queryLang);

                }
                else
                {
                    string culture = _configuration["Globalization:DefaultCulture"];
                    if (!string.IsNullOrWhiteSpace(culture))
                    {
                        defaultCulture = culture;
                        Thread.CurrentThread.SetCulture(culture);
                    }
                }

                var summary = operation.Summary.RemoveNewLineAndTrim();
                if (!string.IsNullOrWhiteSpace(summary))
                {
                    var loc = _localizer[summary];
                    if (loc != summary)
                        operation.Summary = loc;
                }

                var descr = operation.Description.RemoveNewLineAndTrim();
                if (!string.IsNullOrWhiteSpace(descr))
                {
                    var locDescr = _localizer[descr];
                    if (locDescr != descr)
                        operation.Description = locDescr;
                }

                if (operation.Responses != null && operation.Responses.Count > 0)
                {
                    foreach (var r in operation.Responses)
                    {
                        var v = r.Value;
                        if (v != null)
                        {
                            var desc = v.Description.RemoveNewLineAndTrim();
                            if (!string.IsNullOrWhiteSpace(desc))
                            {
                                var loc = _localizer[desc];
                                if (loc != desc)
                                    v.Description = loc;
                            }
                        }
                    }
                }

                if (operation.RequestBody != null)
                {
                    var desc = operation.RequestBody.Description.RemoveNewLineAndTrim();
                    if (!string.IsNullOrWhiteSpace(desc))
                    {
                        var loc = _localizer[desc];
                        if (loc != desc)
                            operation.RequestBody.Description = loc;
                    }
                }

                if (operation.Parameters != null && operation.Parameters.Count > 0)
                {
                    foreach (var r in operation.Parameters)
                    {

                        var desc = r.Description.RemoveNewLineAndTrim();
                        if (!string.IsNullOrWhiteSpace(desc))
                        {
                            var loc = _localizer[desc];
                            if (loc != desc)
                                r.Description = loc;
                        }

                    }
                }
            }
            else
            {
                defaultCulture = _configuration["Globalization:DefaultCulture"];
                if (!string.IsNullOrWhiteSpace(defaultCulture) && enablesAccLang && list != null && list.Count > 0)
                    Thread.CurrentThread.SetCulture(defaultCulture);

            }



            if (enablesAccLang && list != null && list.Count > 0)
            {

                if (!string.IsNullOrWhiteSpace(defaultCulture)) //put default culture in the first position
                    list.Move(x => x.ToLower().Trim() == defaultCulture.Trim().ToLower(), 0);


                if (operation.Parameters == null)
                    operation.Parameters = new List<OpenApiParameter>();

                operation.Parameters.Add(new OpenApiParameter
                {
                    Name = "accept-language",
                    In = ParameterLocation.Header,
                    Required = true,
                    Description = enableLocalizedDoc ? _localizer["supported_languages"] : SwaggerDescr.SupportedLanguages,

                    Schema = new OpenApiSchema
                    {
                        Type = "string",
                        Enum = list.Select(value => (IOpenApiAny)new OpenApiString(value))
                                            .ToList()
                    }

                });
            }

            var httpMethodAttributes = context.MethodInfo
                .GetCustomAttributes(true)
                .OfType<Microsoft.AspNetCore.Mvc.RouteAttribute>();

            var httpMethodWithOptional = httpMethodAttributes?.FirstOrDefault(m => m.Template != null && m.Template.Contains("?"));
            if (httpMethodWithOptional == null)
                return;

            string regex = $"{{(?<{captureName}>\\w+)\\?}}";

            var matches = System.Text.RegularExpressions.Regex.Matches(httpMethodWithOptional.Template, regex);

            foreach (System.Text.RegularExpressions.Match match in matches)
            {
                var name = match.Groups[captureName].Value;

                var parameter = operation.Parameters.FirstOrDefault(p => p.In == ParameterLocation.Path && p.Name == name);
                if (parameter != null)
                {
                    parameter.AllowEmptyValue = true;
                    parameter.Required = false;
                    parameter.Schema.Nullable = true;
                }
            }


        }
    }
    public class SwaggerAddEnumDescriptions : IDocumentFilter
    {
        private readonly IEnumsManager _enumsManager;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SwaggerAddEnumDescriptions(IEnumsManager enumsManager, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _enumsManager = enumsManager;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;

        }
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {

            string queryLang = _httpContextAccessor?.HttpContext.Request?.Query["lang"].FirstOrDefault();
            if (!string.IsNullOrWhiteSpace(queryLang))
                Thread.CurrentThread.SetCulture(queryLang);
            else
            {
                string culture = _configuration["Globalization:DefaultCulture"];
                Thread.CurrentThread.SetCulture(culture);

            }

            // add enum descriptions to result models
            foreach (var property in swaggerDoc.Components.Schemas.Where(x => x.Value?.Enum?.Count > 0))
            {
                IList<IOpenApiAny> propertyEnums = property.Value.Enum;
                if (propertyEnums != null && propertyEnums.Count > 0)
                {
                    property.Value.Description += DescribeEnum(propertyEnums, property.Key);
                }
            }

            // add enum descriptions to input parameters
            foreach (var pathItem in swaggerDoc.Paths)
            {
                DescribeEnumParameters(pathItem.Value.Operations, swaggerDoc, context.ApiDescriptions, pathItem.Key);
            }
        }

        private void DescribeEnumParameters(IDictionary<OperationType, OpenApiOperation> operations, OpenApiDocument swaggerDoc, IEnumerable<ApiDescription> apiDescriptions, string path)
        {
            path = path.Trim('/');
            if (operations != null)
            {
                var pathDescriptions = apiDescriptions.Where(a => a.RelativePath == path);
                foreach (var oper in operations)
                {
                    var operationDescription = pathDescriptions.FirstOrDefault(a => a.HttpMethod.Equals(oper.Key.ToString(), StringComparison.InvariantCultureIgnoreCase));
                    foreach (var param in oper.Value.Parameters)
                    {
                        var parameterDescription = operationDescription.ParameterDescriptions.FirstOrDefault(a => a.Name == param.Name);
                        if (parameterDescription != null && TryGetEnumType(parameterDescription.Type, out Type enumType))
                        {
                            var paramEnum = swaggerDoc.Components.Schemas.FirstOrDefault(x => x.Key == enumType.Name);
                            if (paramEnum.Value != null)
                            {
                                param.Description += DescribeEnum(paramEnum.Value.Enum, paramEnum.Key);
                            }
                        }
                    }
                }
            }
        }

        bool TryGetEnumType(Type type, out Type enumType)
        {
            if (type == null)
            {
                enumType = null;
                return false;

            }

            if (type.IsEnum)
            {
                enumType = type;
                return true;
            }
            else if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                var underlyingType = Nullable.GetUnderlyingType(type);
                if (underlyingType != null && underlyingType.IsEnum)
                {
                    enumType = underlyingType;
                    return true;
                }
            }
            else
            {
                Type underlyingType = GetTypeIEnumerableType(type);
                if (underlyingType != null && underlyingType.IsEnum)
                {
                    enumType = underlyingType;
                    return true;
                }
                else
                {
                    var interfaces = type.GetInterfaces();
                    foreach (var interfaceType in interfaces)
                    {
                        underlyingType = GetTypeIEnumerableType(interfaceType);
                        if (underlyingType != null && underlyingType.IsEnum)
                        {
                            enumType = underlyingType;
                            return true;
                        }
                    }
                }
            }

            enumType = null;
            return false;
        }

        Type GetTypeIEnumerableType(Type type)
        {
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IEnumerable<>))
            {
                var underlyingType = type.GetGenericArguments()[0];
                if (underlyingType.IsEnum)
                {
                    return underlyingType;
                }
            }

            return null;
        }

        private Type GetEnumTypeByName(string enumTypeName)
        {
            return AppDomain.CurrentDomain
                .GetAssemblies()
                .SelectMany(x => x.GetTypes())
                .FirstOrDefault(x => x.Name == enumTypeName);
        }

        private string DescribeEnum(IList<IOpenApiAny> enums, string proprtyTypeName)
        {
            List<string> enumDescriptions = new List<string>();
            var enumType = GetEnumTypeByName(proprtyTypeName);
            if (enumType == null)
                return null;

            foreach (OpenApiInteger enumOption in enums)
            {
                int enumInt = enumOption.Value;

                string name = Enum.GetName(enumType, enumInt);
                string localizedValue = _enumsManager.GetDisplayValue(enumType, name);

                enumDescriptions.Add(string.Format("{0} = {1}", enumInt, $"{name}{(string.IsNullOrWhiteSpace(localizedValue) ? string.Empty : $"({localizedValue})")}"));
            }

            return string.Join(", ", enumDescriptions.ToArray());
        }
    }
}
