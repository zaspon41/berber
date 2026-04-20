namespace API.Controllers;

using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Application.DTOs;
using Application.Interfaces;
using Application.Common;
using Application.Exceptions;

[ApiController]
[Route("api/[controller]")]
public class AuthController : BaseController
{
    private readonly IAdminService _adminService;
    private readonly ILogger<AuthController> _logger;
    private readonly IConfiguration _configuration;

    public AuthController(IAdminService adminService, ILogger<AuthController> logger, IConfiguration configuration)
    {
        _adminService = adminService;
        _logger = logger;
        _configuration = configuration;
    }

    [HttpPost("admin-login")]
    public async Task<ActionResult<ApiResponse<AdminLoginResponse>>> AdminLogin([FromBody] AdminLoginRequest request)
    {
        try
        {
            var result = await _adminService.LoginAsync(request);

            // Generate JWT token
            var token = GenerateJwtToken(result.Id, result.AdminUserName);
            result.Token = token;

            _logger.LogInformation($"Admin {result.AdminUserName} başarıyla giriş yaptı. Token üretildi.");

            return Ok(ApiResponse<AdminLoginResponse>.SuccessResponse(result, "Admin başarıyla giriş yaptı"));
        }
        catch (ValidationException ex)
        {
            _logger.LogWarning($"Admin giriş validasyon hatası: {string.Join(", ", ex.Errors)}");
            return BadRequest(ApiResponse<AdminLoginResponse>.ErrorResponse("Validasyon hatası", ex.Errors));
        }
        catch (UnauthorizedException ex)
        {
            _logger.LogWarning($"Admin giriş başarısız: {ex.Message}");
            return Unauthorized(ApiResponse<AdminLoginResponse>.ErrorResponse(ex.Message));
        }
        catch (NotFoundException ex)
        {
            _logger.LogError($"Admin giriş hatası: {ex.Message}");
            return NotFound(ApiResponse<AdminLoginResponse>.ErrorResponse(ex.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError($"Admin giriş beklenmeyen hata: {ex.Message}");
            return StatusCode(StatusCodes.Status500InternalServerError, 
                ApiResponse<AdminLoginResponse>.ErrorResponse("Beklenmeyen bir hata oluştu"));
        }
    }

    [HttpPost("admin-logout")]
    public ActionResult<ApiResponse> AdminLogout()
    {
        try
        {
            _logger.LogInformation("Admin çıkış isteği alındı (JWT stateless - server tarafında işlem yok)");
            return Ok(ApiResponse.SuccessResponse("Admin başarıyla çıkış yaptı"));
        }
        catch (Exception ex)
        {
            _logger.LogError($"Admin çıkış hatası: {ex.Message}");
            return StatusCode(StatusCodes.Status500InternalServerError, 
                ApiResponse.ErrorResponse("Çıkış yapılırken hata oluştu"));
        }
    }

    [Authorize]
    [HttpGet("admin-status")]
    public ActionResult<ApiResponse<object>> GetAdminStatus()
    {
        try
        {
            _logger.LogInformation("Admin status isteği alındı");

            var adminId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var adminUserName = User.Identity?.Name;

            _logger.LogInformation("Admin status: JWT token doğrulandı. AdminId={AdminId}, AdminUserName={AdminUserName}", adminId, adminUserName);
            return Ok(ApiResponse<object>.SuccessResponse(new { AdminId = adminId, AdminUserName = adminUserName }, "Admin oturumu açık"));
        }
        catch (Exception ex)
        {
            _logger.LogError($"Admin status kontrol hatası: {ex.Message}");
            return StatusCode(StatusCodes.Status500InternalServerError, 
                ApiResponse<object>.ErrorResponse("Beklenmeyen bir hata oluştu"));
        }
    }

    private string GenerateJwtToken(int adminId, string adminUserName)
    {
        var jwtSettings = _configuration.GetSection("Jwt");
        var secretKey = jwtSettings["SecretKey"];
        var issuer = jwtSettings["Issuer"];
        var audience = jwtSettings["Audience"];
        var expirationMinutes = int.Parse(jwtSettings["ExpirationMinutes"] ?? "1440");

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, adminId.ToString()),
            new Claim(ClaimTypes.Name, adminUserName),
            new Claim("AdminId", adminId.ToString())
        };

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(expirationMinutes),
            signingCredentials: credentials
        );

        var tokenHandler = new JwtSecurityTokenHandler();
        var jwtToken = tokenHandler.WriteToken(token);

        _logger.LogInformation($"JWT Token oluşturuldu. AdminId={adminId}, ExpiresIn={expirationMinutes} dakika");

        return jwtToken;
    }
}
