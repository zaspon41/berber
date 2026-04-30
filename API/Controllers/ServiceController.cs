namespace API.Controllers;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.DTOs;
using Application.Exceptions;
using Application.Interfaces;
using Application.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ServiceController : ControllerBase
{
    private readonly IServiceService _serviceService;
    private readonly ILogger<ServiceController> _logger;

    public ServiceController(IServiceService serviceService, ILogger<ServiceController> logger)
    {
        _serviceService = serviceService;
        _logger = logger;
    }

    /// <summary>
    /// Yeni hizmet oluştur
    /// </summary>
    [HttpPost("create")]
    public async Task<IActionResult> CreateService([FromBody] CreateServiceRequest request)
    {
        try
        {
            _logger.LogInformation("Yeni hizmet oluşturma isteneği: {HizmetAdi}, Fiyat: {Fiyat}", 
                request.HizmetAdi, request.Fiyat);

            var result = await _serviceService.CreateServiceAsync(request);

            _logger.LogInformation("Hizmet başarıyla oluşturuldu. ID: {ServiceId}, Adı: {HizmetAdi}", 
                result.Id, result.HizmetAdi);

            return Ok(new ApiResponse<ServiceResponse>
            {
                Success = true,
                Message = "Hizmet başarıyla oluşturuldu",
                Data = result
            });
        }
        catch (ValidationException ex)
        {
            _logger.LogWarning("Hizmet oluşturma doğrulama hatası: {Errors}", string.Join(", ", ex.Errors));
            return BadRequest(new ApiResponse<object>
            {
                Success = false,
                Message = "Doğrulama hatası",
                Errors = ex.Errors
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Hizmet oluşturma sırasında hata oluştu");
            return StatusCode(500, new ApiResponse<object>
            {
                Success = false,
                Message = "Hizmet oluşturma sırasında bir hata oluştu"
            });
        }
    }

    /// <summary>
    /// Tüm hizmetleri listele
    /// </summary>
    [HttpGet("list")]
    [AllowAnonymous]
    public async Task<IActionResult> GetAllServices()
    {
        try
        {
            _logger.LogInformation("Tüm hizmetler listeleniyor");

            var result = await _serviceService.GetAllServicesAsync();

            return Ok(new ApiResponse<List<ServiceResponse>>
            {
                Success = true,
                Message = $"{result.Count} hizmet bulundu",
                Data = result
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Hizmetler listelenirken hata oluştu");
            return StatusCode(500, new ApiResponse<object>
            {
                Success = false,
                Message = "Hizmetler listelenirken bir hata oluştu"
            });
        }
    }

    /// <summary>
    /// ID'ye göre hizmet getir
    /// </summary>
    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetServiceById(int id)
    {
        try
        {
            _logger.LogInformation("Hizmet detayı isteneği: ID {ServiceId}", id);

            var result = await _serviceService.GetServiceByIdAsync(id);

            return Ok(new ApiResponse<ServiceResponse>
            {
                Success = true,
                Message = "Hizmet bulundu",
                Data = result
            });
        }
        catch (NotFoundException ex)
        {
            _logger.LogWarning("Hizmet bulunamadı: {Message}", ex.Message);
            return NotFound(new ApiResponse<object>
            {
                Success = false,
                Message = ex.Message
            });
        }
        catch (BadRequestException ex)
        {
            _logger.LogWarning("Geçersiz istek: {Message}", ex.Message);
            return BadRequest(new ApiResponse<object>
            {
                Success = false,
                Message = ex.Message
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Hizmet detayı alınırken hata oluştu");
            return StatusCode(500, new ApiResponse<object>
            {
                Success = false,
                Message = "Hizmet detayı alınırken bir hata oluştu"
            });
        }
    }

    /// <summary>
    /// Hizmet güncelle
    /// </summary>
    [HttpPut("update")]
    public async Task<IActionResult> UpdateService([FromBody] UpdateServiceRequest request)
    {
        try
        {
            _logger.LogInformation("Hizmet güncelleme isteneği: ID {ServiceId}", request.Id);

            var result = await _serviceService.UpdateServiceAsync(request);

            _logger.LogInformation("Hizmet başarıyla güncellendi. ID: {ServiceId}", result.Id);

            return Ok(new ApiResponse<ServiceResponse>
            {
                Success = true,
                Message = "Hizmet başarıyla güncellendi",
                Data = result
            });
        }
        catch (ValidationException ex)
        {
            _logger.LogWarning("Hizmet güncelleme doğrulama hatası: {Errors}", string.Join(", ", ex.Errors));
            return BadRequest(new ApiResponse<object>
            {
                Success = false,
                Message = "Doğrulama hatası",
                Errors = ex.Errors
            });
        }
        catch (NotFoundException ex)
        {
            _logger.LogWarning("Hizmet bulunamadı: {Message}", ex.Message);
            return NotFound(new ApiResponse<object>
            {
                Success = false,
                Message = ex.Message
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Hizmet güncellemesi sırasında hata oluştu");
            return StatusCode(500, new ApiResponse<object>
            {
                Success = false,
                Message = "Hizmet güncellemesi sırasında bir hata oluştu"
            });
        }
    }

    /// <summary>
    /// Hizmet sil
    /// </summary>
    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> DeleteService(int id)
    {
        try
        {
            _logger.LogInformation("Hizmet silme isteneği: ID {ServiceId}", id);

            var result = await _serviceService.DeleteServiceAsync(id);

            if (result)
            {
                _logger.LogInformation("Hizmet başarıyla silindi. ID: {ServiceId}", id);

                return Ok(new ApiResponse<object>
                {
                    Success = true,
                    Message = "Hizmet başarıyla silindi"
                });
            }

            return NotFound(new ApiResponse<object>
            {
                Success = false,
                Message = "Hizmet silinirken bir hata oluştu"
            });
        }
        catch (NotFoundException ex)
        {
            _logger.LogWarning("Hizmet bulunamadı: {Message}", ex.Message);
            return NotFound(new ApiResponse<object>
            {
                Success = false,
                Message = ex.Message
            });
        }
        catch (BadRequestException ex)
        {
            _logger.LogWarning("Geçersiz istek: {Message}", ex.Message);
            return BadRequest(new ApiResponse<object>
            {
                Success = false,
                Message = ex.Message
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Hizmet silinirken hata oluştu");
            return StatusCode(500, new ApiResponse<object>
            {
                Success = false,
                Message = "Hizmet silinirken bir hata oluştu"
            });
        }
    }
}
