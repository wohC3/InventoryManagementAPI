using InventoryManagement.Models;
using Microsoft.AspNetCore.Mvc;
using InventoryManagement.Service;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
namespace InventoryManagement.Controllers;



[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{

    private readonly AuthService _authService;
    private readonly IConfiguration _configuration;


    public AuthController(AuthService service, IConfiguration iconf)
    {
        _authService = service;
        _configuration = iconf;
    }


    [HttpPost("register")]
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


    [HttpPost("login")]
    public async Task<IActionResult> Login(string username, string password)
    {

        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
        {
            return BadRequest(new
            {
                message = "Validation failed: Username and password fields cannot be empty"
            });
        }


        var user = await _authService.Login(username, password);
        if (user == null)
        {
            return Unauthorized(new
            {
                message = "Wrong credentials!"
            });
        }

        var claims = new List<Claim>
        {
          new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
          new Claim(ClaimTypes.Name, user.Username),
          new Claim(ClaimTypes.Role, user.Role)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var credentials = new SigningCredentials(
            key,
            SecurityAlgorithms.HmacSha256
            );
        var issuer = _configuration["Jwt:Issuer"];
        var audience = _configuration["Jwt:Audience"];
        var expireMinutes = int.Parse(_configuration["Jwt:ExpireMinutes"]);
        var expires = DateTime.UtcNow.AddMinutes(expireMinutes);

        var jwt = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: expires,
            signingCredentials: credentials
            );
        var token = new JwtSecurityTokenHandler().WriteToken(jwt);
        return Ok(new
        {
            token = token
        });

    }
}


