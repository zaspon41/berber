namespace API.Controllers;

using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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

    public AuthController(IAdminService adminService, ILogger<AuthController> logger)
    {
        _adminService = adminService;
        _logger = logger;
    }

    [HttpPost("admin-login")]
    public async Task<ActionResult<ApiResponse<AdminLoginResponse>>> AdminLogin([FromBody] AdminLoginRequest request)
    {
        try
        {
            var result = await _adminService.LoginAsync(request);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, result.Id.ToString()),
                new Claim(ClaimTypes.Name, result.AdminUserName)
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddDays(30),
                AllowRefresh = true
            };

            try
            {
                _logger.LogInformation("SignInAsync başlatılıyor... Schema: {Schema}", CookieAuthenticationDefaults.AuthenticationScheme);
                
                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);

                _logger.LogInformation("SignInAsync başarılı oldu! Cookie yazıldı.");
            }
            catch (Exception signInEx)
            {
                _logger.LogError($"SignInAsync hatası: {signInEx.Message}. Stack: {signInEx.StackTrace}");
                throw;
            }

            _logger.LogInformation($"Admin {result.AdminUserName} başarıyla giriş yaptı");

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
    public async Task<ActionResult<ApiResponse>> AdminLogout()
    {
        try
        {
            var adminUserName = User.Identity?.Name;
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            _logger.LogInformation($"Admin {adminUserName} çıkış yaptı");
            return Ok(ApiResponse.SuccessResponse("Admin başarıyla çıkış yaptı"));
        }
        catch (Exception ex)
        {
            _logger.LogError($"Admin çıkış hatası: {ex.Message}");
            return StatusCode(StatusCodes.Status500InternalServerError, 
                ApiResponse.ErrorResponse("Çıkış yapılırken hata oluştu"));
        }
    }

    [HttpGet("admin-status")]
    public ActionResult<ApiResponse<object>> GetAdminStatus()
    {
        try
        {
            _logger.LogInformation("Admin status isteği alındı");

            var adminId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var adminUserName = User.Identity?.Name;

            if (User.Identity?.IsAuthenticated != true || string.IsNullOrEmpty(adminId))
            {
                _logger.LogWarning("Admin status: oturum bulunamadı");
                return Unauthorized(ApiResponse<object>.ErrorResponse("Admin oturumu açık değil"));
            }

            _logger.LogInformation("Admin status: aktif oturum bulundu. AdminId={AdminId}, AdminUserName={AdminUserName}", adminId, adminUserName);
            return Ok(ApiResponse<object>.SuccessResponse(new { AdminId = adminId, AdminUserName = adminUserName }, "Admin oturumu açık"));
        }
        catch (Exception ex)
        {
            _logger.LogError($"Admin status kontrol hatası: {ex.Message}");
            return StatusCode(StatusCodes.Status500InternalServerError, 
                ApiResponse<object>.ErrorResponse("Beklenmeyen bir hata oluştu"));
        }
    }
}
