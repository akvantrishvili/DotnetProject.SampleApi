
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DotnetProject.SampleApi.Api.Controllers
{
    [ApiController]
    //[ProducesErrorCode(nameof(DefaultErrorCodes.SystemError), StatusCodes.Status500InternalServerError)]
    //[ProducesErrorCode(nameof(DefaultErrorCodes.ValidationError), StatusCodes.Status400BadRequest)]
    //[ProducesErrorCode("ApplicationError", StatusCodes.Status500InternalServerError)]
    //[ProducesErrorCode("DomainError", StatusCodes.Status400BadRequest)]
    //[ProducesErrorCode("ObjectNotFound", StatusCodes.Status404NotFound)]
    public class ApiControllerBase : ControllerBase
    {
    }
}
