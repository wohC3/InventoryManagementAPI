using InventoryManagement.Models;
using Microsoft.AspNetCore.Mvc;
using InventoryManagement.Service;
using Microsoft.AspNetCore.Authorization;
using InventoryManagement.Dtos;
namespace InventoryManagement.Controllers;


[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly UserService _service;

    public UsersController(UserService service)
    {
        _service = service;
    }


    [Authorize(Roles = "Admin")]
    [HttpPut("{id}/role")]
    public async Task<IActionResult> UpdateUserRoleById(int id, UpdateUserRoleDto userRoleDto)
    {
        var userRoleToUpdate = await _service.UpdateUserRole(id, userRoleDto);
        if (userRoleToUpdate == null)
        {
            return NotFound();
        }

        return Ok(userRoleToUpdate);
    }


    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<IActionResult> GetUsers()
    {
        var listOfUsers = await _service.GetAllUsers();
        return Ok(listOfUsers);
    }
}
