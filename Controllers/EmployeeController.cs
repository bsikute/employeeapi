using EmployeeApi.Models;
using EmployeeApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class EmployeeController : ControllerBase
{
    private IEmployeeService _employeeService;
    private readonly ILogger<EmployeeController> _logger;
    public EmployeeController(IEmployeeService employeeService, ILogger<EmployeeController> logger)
    {
        _employeeService = employeeService;
        _logger = logger;
    }
    
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public IEnumerable<EmployeeModel> GetEmployees()
    {
        _logger.LogInformation("Init: GetEmployees");
        return _employeeService.GetAll();
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(EmployeeModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public IActionResult GetEmployeeByid(int id)
    {
        _logger.LogInformation("Init: GetEmployeeById");
        var empl = _employeeService.GetById(id);
        return Ok(empl);
    }

    [HttpPost]
    [ProducesResponseType(typeof(EmployeeModel), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public IActionResult CreateEmployee(EmployeeModel employee)
    {
        _logger.LogInformation("Init: CreateEmployee");
        _employeeService.Create(employee);
        _employeeService.Save();
        return CreatedAtAction(
            nameof(GetEmployeeByid),
            new { id = employee.Id },
            employee
        );
    }

    [HttpDelete("{id}")]
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

    [HttpPut]
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
}