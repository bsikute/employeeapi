using System.Text.Json;
using AutoMapper;
using EmployeeApi.ActionFilters;
using EmployeeApi.Contracts;
using EmployeeApi.DataTransferObject.Models;
using EmployeeApi.Models;
using EmployeeApi.RequestFeatures;
using EmployeeApi.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeApi.Controllers;

[ApiVersion("1.0")]
[ApiController]
[Route("api/{v:apiversion}/[controller]")]
[Authorize]
public class EmployeeController : ControllerBase
{
    private IEmployeeService _employeeService;
    private readonly ILogger<EmployeeController> _logger;
    private readonly IDataShaper<EmployeeDto> _dataShaper;
    private readonly IMapper _mapper;
    private readonly EmployeeLinks _employeeLinks;
    public EmployeeController(IEmployeeService employeeService, ILogger<EmployeeController> logger, IDataShaper<EmployeeDto> dataShaper, IMapper mapper, EmployeeLinks employeeLinks)
    {
        _employeeService = employeeService;
        _logger = logger;
        _dataShaper = dataShaper;
        _mapper = mapper;
        _employeeLinks = employeeLinks;
    }

    [HttpGet("all/", Name = "GetAllEmployees")]
    [HttpHead]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ServiceFilter(typeof(ValidateMediaTypeAttribute))]
    public async Task<IActionResult> GetEmployees([FromQuery] EmployeeParameters employeeParameters)
    {
        _logger.LogInformation("Init: GetEmployees");
        if (!employeeParameters.ValidAgeRange) return BadRequest("Max age can't be less than min age.");
        var employeesFromDb = await _employeeService.GetAll(employeeParameters);
        Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(employeesFromDb.MetaData));

        var employeesDto = _mapper.Map<IEnumerable<EmployeeDto>>(employeesFromDb);

        //return Ok(_dataShaper.ShapeData(employeesDto, employeeParameters.Fields));

        var links = _employeeLinks.TryGenerateLinks(employeesDto, employeeParameters.Fields, HttpContext);
        return links.HasLinks ? Ok(links.LinkedEntities) : Ok(links.ShapedEntities);
    }

    [HttpGet(Name = "GetEmployeeById")]
    [ProducesResponseType(typeof(EmployeeModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public IActionResult GetEmployeeById(int id)
    {
        _logger.LogInformation("Init: GetEmployeeById");
        var empl = _employeeService.GetById(id);
        return Ok(empl);
    }

    [HttpPost(Name = "CreateEmployee")]
    [Authorize(Roles = "Manager")]
    [ProducesResponseType(typeof(EmployeeModel), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ServiceFilter(typeof(ValidateMediaTypeAttribute))]
    public IActionResult CreateEmployee(EmployeeModel employee)
    {
        _logger.LogInformation("Init: CreateEmployee");
        _employeeService.Create(employee);
        _employeeService.Save();
        return CreatedAtAction(
            nameof(GetEmployeeById),
            new { id = employee.Id },
            employee
        );
    }

    [HttpDelete(Name = "DeleteEmployee")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public IActionResult DeleteEmployee(int id)
    {
        _logger.LogInformation("Init: DeleteEmployee");
        _employeeService.Delete(id);
        _employeeService.Save();
        return NoContent();
    }

    [HttpPut(Name = "UpdateEmployee")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public IActionResult UpdateEmployee(int id, EmployeeModel employee)
    {
        _logger.LogInformation("Init: UpdateEmployee");
        _employeeService.Update(employee);
        _employeeService.Save();
        return NoContent();
    }

    [HttpOptions]
    public IActionResult GetEmployeesOptions()
    {
        Response.Headers.Add("Allow", "GET, OPTIONS, POST, DELETE, PUT"); return Ok();
    }
}