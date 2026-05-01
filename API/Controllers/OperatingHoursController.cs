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
public class OperatingHoursController : ControllerBase
{
    private readonly IOperatingHoursService _operatingHoursService;
    private readonly ILogger<OperatingHoursController> _logger;

    public OperatingHoursController(IOperatingHoursService operatingHoursService, ILogger<OperatingHoursController> logger)
    {
        _operatingHoursService = operatingHoursService;
        _logger = logger;
    }

    /// <summary>
    /// Yeni çalışma saati oluştur
    /// </summary>
    [HttpPost("create")]
    public async Task<IActionResult> Create([FromBody] CreateOperatingHoursRequest request)
    {
        try
        {
            _logger.LogInformation("Çalışma saati oluşturma isteği: Gün {day}", request.DayOfWeek);
            var result = await _operatingHoursService.CreateAsync(request);
            return Ok(new ApiResponse<OperatingHoursResponse>
            {
                Success = true,
                Message = "Çalışma saati başarıyla oluşturuldu",
                Data = result
            });
        }
        catch (ValidationException ex)
        {
            _logger.LogWarning("Validasyon hatası: {error}", string.Join(", ", ex.Errors));
            return BadRequest(new ApiResponse<object>
            {
                Success = false,
                Message = "Validasyon hatası",
                Errors = ex.Errors
            });
        }
        catch (Exception ex)
        {
            _logger.LogError("Çalışma saati oluşturma hatası: {error}", ex.Message);
            return StatusCode(500, new ApiResponse<object>
            {
                Success = false,
                Message = "Bir hata meydana geldi",
                Errors = new List<string> { ex.Message }
            });
        }
    }

    /// <summary>
    /// Tüm çalışma saatleri listele
    /// </summary>
    [HttpGet("list")]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            _logger.LogInformation("Çalışma saatleri listeleniyor");
            var result = await _operatingHoursService.GetAllAsync();
            return Ok(new ApiResponse<List<OperatingHoursResponse>>
            {
                Success = true,
                Message = "Çalışma saatleri başarıyla listelendi",
                Data = result
            });
        }
        catch (Exception ex)
        {
            _logger.LogError("Çalışma saatleri listesi hatası: {error}", ex.Message);
            return StatusCode(500, new ApiResponse<object>
            {
                Success = false,
                Message = "Bir hata meydana geldi",
                Errors = new List<string> { ex.Message }
            });
        }
    }

    /// <summary>
    /// ID'ye göre çalışma saatini getir
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            _logger.LogInformation("Çalışma saati getiriliyor: ID {id}", id);
            var result = await _operatingHoursService.GetByIdAsync(id);
            if (result == null)
                return NotFound(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Çalışma saati bulunamadı"
                });
            
            return Ok(new ApiResponse<OperatingHoursResponse>
            {
                Success = true,
                Message = "Çalışma saati başarıyla getirildi",
                Data = result
            });
        }
        catch (NotFoundException ex)
        {
            _logger.LogWarning("Çalışma saati bulunamadı: {error}", ex.Message);
            return NotFound(new ApiResponse<object>
            {
                Success = false,
                Message = ex.Message
            });
        }
        catch (BadRequestException ex)
        {
            _logger.LogWarning("Hatalı istek: {error}", ex.Message);
            return BadRequest(new ApiResponse<object>
            {
                Success = false,
                Message = ex.Message
            });
        }
        catch (Exception ex)
        {
            _logger.LogError("Çalışma saati getirme hatası: {error}", ex.Message);
            return StatusCode(500, new ApiResponse<object>
            {
                Success = false,
                Message = "Bir hata meydana geldi",
                Errors = new List<string> { ex.Message }
            });
        }
    }

    /// <summary>
    /// Çalışma saatini güncelle
    /// </summary>
    [HttpPut("update")]
    public async Task<IActionResult> Update([FromBody] UpdateOperatingHoursRequest request)
    {
        try
        {
            _logger.LogInformation("Çalışma saati güncelleniyor: ID {id}", request.Id);
            var result = await _operatingHoursService.UpdateAsync(request);
            return Ok(new ApiResponse<OperatingHoursResponse>
            {
                Success = true,
                Message = "Çalışma saati başarıyla güncellendi",
                Data = result
            });
        }
        catch (ValidationException ex)
        {
            _logger.LogWarning("Validasyon hatası: {error}", string.Join(", ", ex.Errors));
            return BadRequest(new ApiResponse<object>
            {
                Success = false,
                Message = "Validasyon hatası",
                Errors = ex.Errors
            });
        }
        catch (NotFoundException ex)
        {
            _logger.LogWarning("Çalışma saati bulunamadı: {error}", ex.Message);
            return NotFound(new ApiResponse<object>
            {
                Success = false,
                Message = ex.Message
            });
        }
        catch (BadRequestException ex)
        {
            _logger.LogWarning("Hatalı istek: {error}", ex.Message);
            return BadRequest(new ApiResponse<object>
            {
                Success = false,
                Message = ex.Message
            });
        }
        catch (Exception ex)
        {
            _logger.LogError("Çalışma saati güncelleme hatası: {error}", ex.Message);
            return StatusCode(500, new ApiResponse<object>
            {
                Success = false,
                Message = "Bir hata meydana geldi",
                Errors = new List<string> { ex.Message }
            });
        }
    }

    /// <summary>
    /// Çalışma saatini sil
    /// </summary>
    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            _logger.LogInformation("Çalışma saati siliniyor: ID {id}", id);
            var result = await _operatingHoursService.DeleteAsync(id);
            return Ok(new ApiResponse<bool>
            {
                Success = true,
                Message = "Çalışma saati başarıyla silindi",
                Data = result
            });
        }
        catch (NotFoundException ex)
        {
            _logger.LogWarning("Çalışma saati bulunamadı: {error}", ex.Message);
            return NotFound(new ApiResponse<object>
            {
                Success = false,
                Message = ex.Message
            });
        }
        catch (BadRequestException ex)
        {
            _logger.LogWarning("Hatalı istek: {error}", ex.Message);
            return BadRequest(new ApiResponse<object>
            {
                Success = false,
                Message = ex.Message
            });
        }
        catch (Exception ex)
        {
            _logger.LogError("Çalışma saati silme hatası: {error}", ex.Message);
            return StatusCode(500, new ApiResponse<object>
            {
                Success = false,
                Message = "Bir hata meydana geldi",
                Errors = new List<string> { ex.Message }
            });
        }
    }

    /// <summary>
    /// Verilen tarihte müsait saatleri göster (Public)
    /// </summary>
    [AllowAnonymous]
    [HttpGet("available/{date}")]
    public async Task<IActionResult> GetAvailableHours(string date)
    {
        try
        {
            if (!DateTime.TryParse(date, out var selectedDate))
                return BadRequest(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Geçersiz tarih formatı (yyyy-MM-dd)"
                });

            var dayOfWeek = (int)selectedDate.DayOfWeek;
            _logger.LogInformation("Müsait saatler sorgulanıyor: {date}, Gün: {day}", selectedDate.Date, dayOfWeek);
            
            var hours = await _operatingHoursService.GetByDayAsync(dayOfWeek);
            
            if (hours == null)
                return Ok(new ApiResponse<object>
                {
                    Success = true,
                    Message = "Bu gün kapalı",
                    Data = null
                });

            return Ok(new ApiResponse<OperatingHoursResponse>
            {
                Success = true,
                Message = "Müsait saatler",
                Data = hours
            });
        }
        catch (Exception ex)
        {
            _logger.LogError("Müsait saatler sorgu hatası: {error}", ex.Message);
            return StatusCode(500, new ApiResponse<object>
            {
                Success = false,
                Message = "Saatler getirilirken hata oluştu"
            });
        }
    }
}
