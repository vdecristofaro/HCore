﻿using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using ReinhardHolzner.Core.Web.Exceptions;
using System;
using System.Threading.Tasks;

namespace ReinhardHolzner.Core.Web.Middleware
{
    internal class UnhandledExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<UnhandledExceptionHandlingMiddleware> _logger;

        public UnhandledExceptionHandlingMiddleware(RequestDelegate next, ILogger<UnhandledExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task Invoke(HttpContext context)
        {
            ApiException resultException = null;

            try
            {
                await _next.Invoke(context).ConfigureAwait(false);
            }
            catch (ApiException e)
            {
                resultException = e;
            }
            catch (NotImplementedException e)
            {
                _logger.LogError($"Not implemented exception: {e}");

                resultException = new NotImplementedApiException();
            }
            catch (Exception e)
            {
                _logger.LogError($"Unexpected server error: {e}");

                resultException = new InternalServerErrorApiException();                
            }            

            if (resultException != null)            
                await resultException.WriteResponseAsync(context).ConfigureAwait(false);            
        }
    }
}