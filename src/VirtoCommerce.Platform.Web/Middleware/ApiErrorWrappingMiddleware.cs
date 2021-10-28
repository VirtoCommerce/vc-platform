using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;

namespace VirtoCommerce.Platform.Web.Middleware
{
    public class ApiErrorWrappingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ApiErrorWrappingMiddleware> _logger;
        private readonly IWebHostEnvironment _env;

        public ApiErrorWrappingMiddleware(RequestDelegate next, ILogger<ApiErrorWrappingMiddleware> logger, IWebHostEnvironment env)
        {
            _next = next;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _env = env;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
            }
            catch (Exception ex)
            {
                //Need handle only storefront api errors
                if (!context.Response.HasStarted && context.Request.Path.ToString().Contains("/api/"))
                {
                    var isDevelopment = _env.IsDevelopment();
                    var shortMessage = ex.InnerException == null ? ex.Message : ex.InnerException.Message;
                    var message = !isDevelopment ? shortMessage : $@"An exception occurred while processing the request [{context.Request.Path}]: {ex}";
                    _logger.LogError(ex, message);
                    var httpStatusCode = HttpStatusCode.InternalServerError;
                    var json = JsonConvert.SerializeObject(new { message, stackTrace = isDevelopment ? ex.StackTrace : null });
                    context.Response.ContentType = "application/json";
                    context.Response.StatusCode = (int)httpStatusCode;
                    await context.Response.WriteAsync(json);
                }
                else
                {
                    //Continue default error handling
                    throw;
                }
            }
        }
    }
}
