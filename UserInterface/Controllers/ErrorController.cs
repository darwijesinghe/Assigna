using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using UserInterface.Models;

namespace UserInterface.Controllers
{
    public class ErrorController : Controller
    {
        // services
        private readonly ILogger<ErrorController> _logger;

        public ErrorController(ILogger<ErrorController> logger)
        {
            _logger = logger;
        }

        // unhandle exceptions
        [HttpGet]
        [Route("error")]
        public IActionResult Error()
        {
            // error infomation
            var error = HttpContext
            .Features
            .Get<IExceptionHandlerPathFeature>();

            // loggin error
            _logger.LogError(error.Error.Message);

            return View();
        }

        // http errors
        [HttpGet]
        [Route("error/{statuscode}")]
        public IActionResult HttpStatusCodeHandler(int statuscode = 0)
        {
            // http error infomation
            var exception = HttpContext
            .Features
            .Get<IExceptionHandlerPathFeature>();

            if (exception is not null)
            {
                // loggin error
                _logger.LogError(exception.Error.Message);
            }


            ErrorViewModel? error = new ErrorViewModel();

            // handle error page based on http status code
            switch (statuscode)
            {
                case 404:
                    error.status = 404;
                    break;
                case 500:
                    error.status = 500;
                    break;
            }

            return View("HttpError", error);
        }
    }
}
