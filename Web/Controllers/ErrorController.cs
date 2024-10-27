using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using UserInterface.Models;

namespace UserInterface.Controllers
{
    public class ErrorController : Controller
    {
        // Services
        private readonly ILogger<ErrorController> _logger;

        public ErrorController(ILogger<ErrorController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Handles application errors and displays the error page.
        /// </summary>
        /// <returns>
        /// An <see cref="IActionResult"/> that renders the error page.
        /// </returns>
        [HttpGet]
        [Route("error")]
        public IActionResult Error()
        {
            // error infomation
            var error = HttpContext.Features.Get<IExceptionHandlerPathFeature>();

            // loggin error
            _logger.LogError(error.Error.Message);

            return View();
        }

        /// <summary>
        /// Handles HTTP status codes and returns an appropriate response based on the provided status code.
        /// </summary>
        /// <param name="statuscode">The HTTP status code to handle. Defaults to 0, which may indicate a general error.</param>
        /// <returns>
        /// An <see cref="IActionResult"/> representing the response corresponding to the specified HTTP status code.
        /// </returns>
        [HttpGet]
        [Route("error/{statuscode}")]
        public IActionResult HttpStatusCodeHandler(int statuscode = 0)
        {
            // http error infomation
            var exception = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            if (exception is not null)
            {
                // loggin error
                _logger.LogError(exception.Error.Message);
            }

            // view data
            var error = new ErrorViewModel();

            // handles error page based on http status code
            switch (statuscode)
            {
                case 404:
                    error.Status = 404;
                    break;
                case 500:
                    error.Status = 500;
                    break;
            }

            return View("HttpError", error);
        }
    }
}
