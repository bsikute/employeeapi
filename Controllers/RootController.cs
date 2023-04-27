using EmployeeApi.Models.LinksModels;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeApi.Controllers;

[Route("api/{v:apiversion}/")]
[ApiController]
public class RootController : ControllerBase
{
    private readonly LinkGenerator _linkGenerator;
    public RootController(LinkGenerator linkGenerator)
    {
        _linkGenerator = linkGenerator;
    }

    [HttpGet(Name = "GetRoot")]
    public IActionResult GetRoot([FromHeader(Name = "Accept")] string mediaType)
    {
        if (mediaType.Contains("application/vnd.codemaze.apiroot"))
        {
            var list = new List<Link>
            {
                new Link
                {
                    Href = _linkGenerator.GetUriByName(HttpContext, nameof(GetRoot),new {}),
                    Rel = "self",
                    Method = "GET"
                },
                new Link
                {
                    Href = _linkGenerator.GetUriByName(HttpContext, "GetAllEmployees", new {}),
                    Rel = "get_all_employees",
                    Method = "GET"
                },
                new Link
                {
                    Href = _linkGenerator.GetUriByName(HttpContext, "GetEmployeeById", new {}),
                    Rel = "get_employee",
                    Method = "GET"
                },
                new Link
                {
                    Href = _linkGenerator.GetUriByName(HttpContext, "CreateEmployee", new {}),
                    Rel = "create_employee",
                    Method = "POST"
                },
                new Link
                {
                    Href = _linkGenerator.GetUriByName(HttpContext, "UpdateEmployee", new {}),
                    Rel = "update_employee",
                    Method = "PUT"
                },
                new Link
                {
                    Href = _linkGenerator.GetUriByName(HttpContext, "DeleteEmployee", new {}),
                    Rel = "delete_employee",
                    Method = "DELETE"
                }
            };
            return Ok(list);
        }
        return NoContent();
    }
}