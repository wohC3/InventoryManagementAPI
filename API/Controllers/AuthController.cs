using InventoryManagement.Models;
using Microsoft.AspNetCore.Mvc;
using InventoryManagement.Service;
namespace InventoryManagement.Controllers;


[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{

    private readonly AuthService _authService;

    public AuthController(AuthService service)
    {
        _authService = service;
    }


    [HttpPost]
    public async Task<IActionResult> Register(string username, string password)
    {
        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
        {
            return BadRequest(new
            {
                message = "Validation failed: Username and password fields cannot be empty"
            });
        }
        var result = await _authService.Register(username, password);
        if (!result)
        {
            return BadRequest(new
            {
                message = "Username already exists!"
            });
        }
        return StatusCode(201, new
        {
            message = "Successfully registered!"
        });
    }


}


