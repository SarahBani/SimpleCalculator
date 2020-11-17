using Microsoft.AspNetCore.Mvc;
using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Common
{
    public static class Helper
    {

        #region Properties

        private static readonly object _locker = new object();

        private static HttpClient _client;

        #endregion /Properties

        #region Methods

        public static HttpClient GetClient(string baseAddress)
        {
            if (_client == null)
            {
                lock (_locker)
                {
                    if (_client == null)
                    {
                        _client = new HttpClient();
                        _client.BaseAddress = new Uri(baseAddress);
                        _client.DefaultRequestHeaders.Accept.Clear();
                        // Add an Accept header for JSON format.
                        _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(Constant.MediaType));

                    }
                }
            }
            return _client;
        }

        /// <summary>
        /// a customized BadRequestObjectResult
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static IActionResult GetBadRequestResult(string message) =>
            new BadRequestObjectResult(Constant.ReturnChar + message);

        #endregion /Methods

    }
}
