using EmployeeApi.Contracts;
using EmployeeApi.DataTransferObject.Models;
using EmployeeApi.Models;
using EmployeeApi.Models.LinksModels;
using Microsoft.Net.Http.Headers;

namespace EmployeeApi.Utility;

public class EmployeeLinks
{
    private readonly LinkGenerator _linkGenerator;
    private readonly IDataShaper<EmployeeDto> _dataShaper;
    public EmployeeLinks(LinkGenerator linkGenerator, IDataShaper<EmployeeDto> dataShaper)
    {
        _linkGenerator = linkGenerator;
        _dataShaper = dataShaper;
    }

    public LinkResponse TryGenerateLinks(IEnumerable<EmployeeDto> employeeDto, string? fields, HttpContext httpContext)
    {
        var shapedEmployees = ShapeData(employeeDto, fields);

        if (ShouldGenerateLinks(httpContext))
            return ReturnLinkdedEmployees(employeeDto, fields, httpContext, shapedEmployees);
        
        return ReturnShapedEmployees(shapedEmployees);
    }

    private List<Entity> ShapeData(IEnumerable<EmployeeDto> employeeDto, string fields) =>
    _dataShaper.ShapeData(employeeDto, fields).Select(e => e.Entity).ToList();

    private bool ShouldGenerateLinks(HttpContext httpContext)
    {
        var mediaType = (MediaTypeHeaderValue)httpContext.Items["AcceptHeaderMediaType"];
        return mediaType.SubTypeWithoutSuffix.EndsWith("hateoas", StringComparison.InvariantCultureIgnoreCase);
    }
    private LinkResponse ReturnShapedEmployees(List<Entity> shapedEmployees) => new LinkResponse
    { ShapedEntities = shapedEmployees };

    private LinkResponse ReturnLinkdedEmployees(IEnumerable<EmployeeDto> employeesDto, string fields, HttpContext httpContext, List<Entity> shapedEmployees)
    {
        var employeeDtoList = employeesDto.ToList();
        for (var index = 0; index < employeeDtoList.Count(); index++)
        {
            var employeeLinks = CreateLinksForEmployee(httpContext, employeeDtoList[index].Id, fields);
            shapedEmployees[index].Add("Links", employeeLinks);
        }
        var employeeCollection = new LinkCollectionWrapper<Entity>(shapedEmployees);
        var linkedEmployees = CreateLinksForEmployees(httpContext, employeeCollection);
        return new LinkResponse { HasLinks = true, LinkedEntities = linkedEmployees };
    }

    private List<Link> CreateLinksForEmployee(HttpContext httpContext, int id, string fields = "")
    {
        var links = new List<Link>
        {
            new Link(_linkGenerator.GetUriByAction(httpContext, "GetEmployeeById", values: new { id }), "self", "GET"),
            new Link(_linkGenerator.GetUriByAction(httpContext, "DeleteEmployee", values: new { id }), "delete_employee", "DELETE"),
            new Link(_linkGenerator.GetUriByAction(httpContext, "UpdateEmployee", values: new { id }), "update_employee", "PUT"),
        };
        return links;
    }
    private LinkCollectionWrapper<Entity> CreateLinksForEmployees(HttpContext httpContext, LinkCollectionWrapper<Entity> employeesWrapper)
    {
        employeesWrapper.Links.Add(new Link(_linkGenerator.GetUriByAction(httpContext, "GetEmployeesForCompany", values: new { }), "self", "GET"));
        return employeesWrapper;
    }
}