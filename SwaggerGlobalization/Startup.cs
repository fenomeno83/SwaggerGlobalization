using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using SwaggerGlobalization.Infrastructure.Extensions;
using SwaggerGlobalization.Infrastructure.Managers;
using SwaggerGlobalization.Infrastructure.Middlewares;
using SwaggerGlobalization.Interfaces;
using SwaggerGlobalization.Models;
using SwaggerGlobalization.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Xml;
using Microsoft.Extensions.Localization;
using System.Threading;
using System.Xml.XPath;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace SwaggerGlobalization
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        private void LocalizeSwaggerText(XmlNode e, IStringLocalizer<Resources> localizer)
        {
            var text = e.InnerText.RemoveNewLineAndTrim();
            var loc = localizer[text];
            if (loc != text)
                e.InnerText = loc;

        }
        private IStringLocalizer<Resources> GetSwaggerStringLocalizer(IServiceCollection services)
        {
            return services.BuildServiceProvider().GetService<IStringLocalizer<Resources>>();
        }


        public void ConfigureServices(IServiceCollection services)
        {
           
            services.AddControllers()
                    .ConfigureApiBehaviorOptions(c => c.SuppressModelStateInvalidFilter = true)
                    .AddNewtonsoftJson(
                            options =>
                            {
                                options.SerializerSettings.ContractResolver = new DefaultContractResolver(); //enable pascal case
                                options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
                                options.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;
                            })
                    .AddJsonOptions(opt => opt.JsonSerializerOptions.PropertyNamingPolicy = null) //enable pascal case in swagger
                    .AddDataAnnotationsLocalization //model data annotation/validation using localization resources
                    (
                        options =>
                        {
                            options.DataAnnotationLocalizerProvider = (type, factory) => factory.Create(typeof(Resources));
                        }
                    );

           
          
            services.AddLocalization(o =>
            {
                // localization in separated assembly
                o.ResourcesPath = "Resources";
            });



            services.AddSwaggerGen(c =>
            {
                var list = Configuration.GetSection("Globalization:SupportedCultures").Get<List<string>>().Where(x => !string.IsNullOrWhiteSpace(x)).Distinct().ToList();
                string defaultCulture = Configuration["Globalization:DefaultCulture"];
                bool.TryParse(Configuration["Globalization:Swagger:EnableMultilanguageDocumentation"], out bool enableMultiLangSwagger);
                bool.TryParse(Configuration["Globalization:Swagger:EnableLocalizedDocumentation"], out bool enableLocalizedDoc);

                var localizer = GetSwaggerStringLocalizer(services);

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

                if (enableMultiLangSwagger && list != null && list.Count > 1)
                {
                    foreach (var l in list)
                    {

                        Thread.CurrentThread.SetCulture(l);

                        c.SwaggerDoc($"v1-{l.Trim()}", new OpenApiInfo
                        {
                            Title = $"Swagger ({l.Trim()})",
                            Version = $"v1-{l.Trim()}",
                            Description = localizer["swagger_description"] //replace with a const if you don't want localize api description
                        });

                    }

                    if (enableLocalizedDoc)
                    {
                        var xmlDoc = new XPathDocument(xmlPath);

                        c.ParameterFilter<XmlCommentsParameterFilter>(xmlDoc);
                        c.RequestBodyFilter<XmlCommentsRequestBodyFilter>(xmlDoc);
                        c.OperationFilter<XmlCommentsOperationFilter>(xmlDoc);
                        //commented because included in OperationFilter
                        //c.ParameterFilter<CustomParameterFilter>();
                        //c.RequestBodyFilter<CustomRequestBodyFilter>();
                    }
                }
                else
                {
                    Thread.CurrentThread.SetCulture(defaultCulture);

                    c.SwaggerDoc("v1", new OpenApiInfo
                    {
                        Title = "Swagger",
                        Version = "v1",
                        Description = localizer["swagger_description"] //replace with a const if you don't want localize api description
                    });

                    if (enableLocalizedDoc)
                    {
                        var doc = new XmlDocument();
                        doc.Load(xmlPath);

                        //localize comments
                        var nodeList = doc.GetElementsByTagName("summary").Cast<XmlNode>().Concat<XmlNode>(doc.GetElementsByTagName("remarks").Cast<XmlNode>()).Concat<XmlNode>(doc.GetElementsByTagName("param").Cast<XmlNode>())
                                        .Concat<XmlNode>(doc.GetElementsByTagName("response").Cast<XmlNode>());

                        foreach (XmlNode e in nodeList)
                        {
                            LocalizeSwaggerText(e, localizer);
                        }

                        Stream stream = new MemoryStream();
                        doc.Save(stream);
                        stream.Position = 0;
                        c.IncludeXmlComments(() => { return new XPathDocument(stream); });
                    }
                }

                if (!enableLocalizedDoc)
                    c.IncludeXmlComments(xmlPath);

                c.OperationFilter<CustomOperationFilter>();

                c.DocumentFilter<SwaggerAddEnumDescriptions>(); //enums custom management

              
            });

            services.AddHttpContextAccessor();

            services.AddSingleton<IEnumsManager, EnumsManager>();

            services.AddBusinessServices();


        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider provider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseBaseMiddleware();

            //give services access also to static methods, where you can't use dependency injection. You shouldn't use it. Prefer dependecy injection and services methods
            //AppServicesHelper.Services = app.ApplicationServices;

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseCors(cpb =>
            {
                cpb.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();

            });

           
            app.UseLocalizationMiddleware(Configuration);

            app.ConfigureExceptionHandler();

            app.UseStaticFiles();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                var list = Configuration.GetSection("Globalization:SupportedCultures").Get<List<string>>().Where(x => !string.IsNullOrWhiteSpace(x)).Distinct().ToList();
                string defaultCulture = Configuration["Globalization:DefaultCulture"];
                bool.TryParse(Configuration["Globalization:Swagger:EnableMultilanguageDocumentation"], out bool enableMultiLangSwagger);
                bool.TryParse(Configuration["Globalization:Swagger:EnableUITranslation"], out bool enableUITranslation);


                if (enableMultiLangSwagger && list != null && list.Count > 1)
                {
                    if (!string.IsNullOrWhiteSpace(defaultCulture)) // put in first position default language version
                        list.Move(x => x.ToLower().Trim() == defaultCulture.ToLower().Trim(), 0);

                    foreach (var l in list)
                    {
                        c.SwaggerEndpoint($"/swagger/v1-{l.Trim()}/swagger.json?lang={l.Trim()}", $"Swagger ({l.Trim()})");
                    }

                    if (enableUITranslation)
                        c.HeadContent = "<script src='./script/jquery-3.3.1.min.js'></script><script src='./script/jquery.initialize.min.js'></script><script src='./script/translate/translate.js'></script>";

                }
                else
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Swagger");

                    if (!string.IsNullOrWhiteSpace(defaultCulture) && enableUITranslation)
                        c.HeadContent = "<script src='./script/jquery-3.3.1.min.js'></script><script src='./jscript/query.initialize.min.js'></script><script id='languagefile' src='./script/translate/" + defaultCulture.Trim() + ".js'></script><script src='./script/translate/translate.js'></script>";
                }

                c.RoutePrefix = string.Empty;
            });


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

        }
    }
}
