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
public class BlockedDatesController : ControllerBase
{
    private readonly IBlockedDatesService _blockedDatesService;
    private readonly ILogger<BlockedDatesController> _logger;

    public BlockedDatesController(IBlockedDatesService blockedDatesService, ILogger<BlockedDatesController> logger)
    {
        _blockedDatesService = blockedDatesService;
        _logger = logger;
    }

    /// <summary>
    /// Yeni kapalı gün oluştur
    /// </summary>
    [HttpPost("create")]
    public async Task<IActionResult> Create([FromBody] CreateBlockedDatesRequest request)
    {
        try
        {
            _logger.LogInformation("Kapalı gün oluşturma isteği: Tarih {date}", request.Tarih);
            var result = await _blockedDatesService.CreateAsync(request);
            return Ok(new ApiResponse<BlockedDatesResponse>
            {
                Success = true,
                Message = "Kapalı gün başarıyla oluşturuldu",
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
            _logger.LogError("Kapalı gün oluşturma hatası: {error}", ex.Message);
            return StatusCode(500, new ApiResponse<object>
            {
                Success = false,
                Message = "Bir hata meydana geldi",
                Errors = new List<string> { ex.Message }
            });
        }
    }

    /// <summary>
    /// Tüm kapalı günleri listele
    /// </summary>
    [HttpGet("list")]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            _logger.LogInformation("Kapalı günler listeleniyor");
            var result = await _blockedDatesService.GetAllAsync();
            return Ok(new ApiResponse<List<BlockedDatesResponse>>
            {
                Success = true,
                Message = "Kapalı günler başarıyla listelendi",
                Data = result
            });
        }
        catch (Exception ex)
        {
            _logger.LogError("Kapalı günler listesi hatası: {error}", ex.Message);
            return StatusCode(500, new ApiResponse<object>
            {
                Success = false,
                Message = "Bir hata meydana geldi",
                Errors = new List<string> { ex.Message }
            });
        }
    }

    /// <summary>
    /// ID'ye göre kapalı günü getir
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            _logger.LogInformation("Kapalı gün getiriliyor: ID {id}", id);
            var result = await _blockedDatesService.GetByIdAsync(id);
            if (result == null)
                return NotFound(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Kapalı gün bulunamadı"
                });
            
            return Ok(new ApiResponse<BlockedDatesResponse>
            {
                Success = true,
                Message = "Kapalı gün başarıyla getirildi",
                Data = result
            });
        }
        catch (NotFoundException ex)
        {
            _logger.LogWarning("Kapalı gün bulunamadı: {error}", ex.Message);
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
            _logger.LogError("Kapalı gün getirme hatası: {error}", ex.Message);
            return StatusCode(500, new ApiResponse<object>
            {
                Success = false,
                Message = "Bir hata meydana geldi",
                Errors = new List<string> { ex.Message }
            });
        }
    }

    /// <summary>
    /// Kapalı günü güncelle
    /// </summary>
    [HttpPut("update")]
    public async Task<IActionResult> Update([FromBody] UpdateBlockedDatesRequest request)
    {
        try
        {
            _logger.LogInformation("Kapalı gün güncelleniyor: ID {id}", request.Id);
            var result = await _blockedDatesService.UpdateAsync(request);
            return Ok(new ApiResponse<BlockedDatesResponse>
            {
                Success = true,
                Message = "Kapalı gün başarıyla güncellendi",
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
            _logger.LogWarning("Kapalı gün bulunamadı: {error}", ex.Message);
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
            _logger.LogError("Kapalı gün güncelleme hatası: {error}", ex.Message);
            return StatusCode(500, new ApiResponse<object>
            {
                Success = false,
                Message = "Bir hata meydana geldi",
                Errors = new List<string> { ex.Message }
            });
        }
    }

    /// <summary>
    /// Kapalı günü sil
    /// </summary>
    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            _logger.LogInformation("Kapalı gün siliniyor: ID {id}", id);
            var result = await _blockedDatesService.DeleteAsync(id);
            return Ok(new ApiResponse<bool>
            {
                Success = true,
                Message = "Kapalı gün başarıyla silindi",
                Data = result
            });
        }
        catch (NotFoundException ex)
        {
            _logger.LogWarning("Kapalı gün bulunamadı: {error}", ex.Message);
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
            _logger.LogError("Kapalı gün silme hatası: {error}", ex.Message);
            return StatusCode(500, new ApiResponse<object>
            {
                Success = false,
                Message = "Bir hata meydana geldi",
                Errors = new List<string> { ex.Message }
            });
        }
    }

    /// <summary>
    /// Belirtilen ay içindeki kapalı günleri göster (Public)
    /// </summary>
    [AllowAnonymous]
    [HttpGet("month/{year}/{month}")]
    public async Task<IActionResult> GetBlockedDatesInMonth(int year, int month)
    {
        try
        {
            if (month < 1 || month > 12)
                return BadRequest(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Ay 1-12 arasında olmalıdır"
                });

            _logger.LogInformation("Kapalı günler sorgulanıyor: {year}-{month}", year, month);
            
            var firstDay = new DateOnly(year, month, 1);
            var lastDay = firstDay.AddMonths(1).AddDays(-1);

            var blockedDates = await _blockedDatesService.GetByDateRangeAsync(firstDay, lastDay);

            return Ok(new ApiResponse<List<BlockedDatesResponse>>
            {
                Success = true,
                Message = $"{year}-{month:D2} ayının kapalı günleri",
                Data = blockedDates
            });
        }
        catch (Exception ex)
        {
            _logger.LogError("Kapalı günler sorgu hatası: {error}", ex.Message);
            return StatusCode(500, new ApiResponse<object>
            {
                Success = false,
                Message = "Kapalı günler getirilirken hata oluştu"
            });
        }
    }
}
