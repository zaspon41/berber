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
public class AppointmentController : ControllerBase
{
    private readonly IAppointmentService _appointmentService;
    private readonly ILogger<AppointmentController> _logger;

    public AppointmentController(IAppointmentService appointmentService, ILogger<AppointmentController> logger)
    {
        _appointmentService = appointmentService;
        _logger = logger;
    }

    /// <summary>
    /// Yeni randevu oluştur
    /// </summary>
    [HttpPost("create")]
    public async Task<IActionResult> Create([FromBody] CreateAppointmentRequest request)
    {
        try
        {
            _logger.LogInformation("Randevu oluşturma isteği: {customerName} - {date}", request.MüşteriAdı, request.RandevuTarihi);
            var result = await _appointmentService.CreateAsync(request);
            return Ok(new ApiResponse<AppointmentResponse>
            {
                Success = true,
                Message = "Randevu başarıyla oluşturuldu",
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
            _logger.LogError("Randevu oluşturma hatası: {error}", ex.Message);
            return StatusCode(500, new ApiResponse<object>
            {
                Success = false,
                Message = "Bir hata meydana geldi",
                Errors = new List<string> { ex.Message }
            });
        }
    }

    /// <summary>
    /// Tüm randevuları listele
    /// </summary>
    [HttpGet("list")]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            _logger.LogInformation("Randevular listeleniyor");
            var result = await _appointmentService.GetAllAsync();
            return Ok(new ApiResponse<List<AppointmentResponse>>
            {
                Success = true,
                Message = "Randevular başarıyla listelendi",
                Data = result
            });
        }
        catch (Exception ex)
        {
            _logger.LogError("Randevular listesi hatası: {error}", ex.Message);
            return StatusCode(500, new ApiResponse<object>
            {
                Success = false,
                Message = "Bir hata meydana geldi",
                Errors = new List<string> { ex.Message }
            });
        }
    }

    /// <summary>
    /// ID'ye göre randevuyu getir
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            _logger.LogInformation("Randevu getiriliyor: ID {id}", id);
            var result = await _appointmentService.GetByIdAsync(id);
            if (result == null)
                return NotFound(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Randevu bulunamadı"
                });
            
            return Ok(new ApiResponse<AppointmentResponse>
            {
                Success = true,
                Message = "Randevu başarıyla getirildi",
                Data = result
            });
        }
        catch (NotFoundException ex)
        {
            _logger.LogWarning("Randevu bulunamadı: {error}", ex.Message);
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
            _logger.LogError("Randevu getirme hatası: {error}", ex.Message);
            return StatusCode(500, new ApiResponse<object>
            {
                Success = false,
                Message = "Bir hata meydana geldi",
                Errors = new List<string> { ex.Message }
            });
        }
    }

    /// <summary>
    /// Randevuyu güncelle
    /// </summary>
    [HttpPut("update")]
    public async Task<IActionResult> Update([FromBody] UpdateAppointmentRequest request)
    {
        try
        {
            _logger.LogInformation("Randevu güncelleniyor: ID {id}", request.Id);
            var result = await _appointmentService.UpdateAsync(request);
            return Ok(new ApiResponse<AppointmentResponse>
            {
                Success = true,
                Message = "Randevu başarıyla güncellendi",
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
            _logger.LogWarning("Randevu bulunamadı: {error}", ex.Message);
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
            _logger.LogError("Randevu güncelleme hatası: {error}", ex.Message);
            return StatusCode(500, new ApiResponse<object>
            {
                Success = false,
                Message = "Bir hata meydana geldi",
                Errors = new List<string> { ex.Message }
            });
        }
    }

    /// <summary>
    /// Randevuyu sil
    /// </summary>
    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            _logger.LogInformation("Randevu siliniyor: ID {id}", id);
            var result = await _appointmentService.DeleteAsync(id);
            return Ok(new ApiResponse<bool>
            {
                Success = true,
                Message = "Randevu başarıyla silindi",
                Data = result
            });
        }
        catch (NotFoundException ex)
        {
            _logger.LogWarning("Randevu bulunamadı: {error}", ex.Message);
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
            _logger.LogError("Randevu silme hatası: {error}", ex.Message);
            return StatusCode(500, new ApiResponse<object>
            {
                Success = false,
                Message = "Bir hata meydana geldi",
                Errors = new List<string> { ex.Message }
            });
        }
    }

    /// <summary>
    /// Müşteri randevu oluştur (Public - kimlik kontrolü yok)
    /// </summary>
    [AllowAnonymous]
    [HttpPost("book")]
    public async Task<IActionResult> BookAppointment([FromBody] CreateAppointmentRequest request)
    {
        try
        {
            _logger.LogInformation("Müşteri randevu talebinde: {customerName} - {date}", request.MüşteriAdı, request.RandevuTarihi);
            var result = await _appointmentService.CreateAsync(request);
            return Ok(new ApiResponse<AppointmentResponse>
            {
                Success = true,
                Message = "Randevunuz başarıyla oluşturuldu. Bize ulaşın.",
                Data = result
            });
        }
        catch (ValidationException ex)
        {
            _logger.LogWarning("Validasyon hatası: {error}", string.Join(", ", ex.Errors));
            return BadRequest(new ApiResponse<object>
            {
                Success = false,
                Message = "Lütfen bilgilerinizi kontrol ediniz",
                Errors = ex.Errors
            });
        }
        catch (BadRequestException ex)
        {
            _logger.LogWarning("Uygun olmayan istek: {error}", ex.Message);
            return BadRequest(new ApiResponse<object>
            {
                Success = false,
                Message = ex.Message
            });
        }
        catch (Exception ex)
        {
            _logger.LogError("Randevu booking hatası: {error}", ex.Message);
            return StatusCode(500, new ApiResponse<object>
            {
                Success = false,
                Message = "Randevu oluşturulurken hata oluştu, lütfen tekrar deneyin"
            });
        }
    }

    /// <summary>
    /// Müşteri randevuyu iptal et (Public)
    /// </summary>
    [AllowAnonymous]
    [HttpPut("cancel/{id}")]
    public async Task<IActionResult> CancelAppointment(int id)
    {
        try
        {
            _logger.LogInformation("Randevu iptal isteneği: ID {id}", id);
            
            // Randevuyu getir
            var appointment = await _appointmentService.GetByIdAsync(id);
            
            // Durumu kontrol et
            if (appointment.Durum == "İptal")
                return BadRequest(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Bu randevu zaten iptal edilmiş"
                });

            // Geçmiş randevu iptal edilemesin
            if (DateTime.Now > appointment.RandevuTarihi)
                return BadRequest(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Geçmiş randevular iptal edilemez"
                });

            // Status'u iptal et
            var updateRequest = new UpdateAppointmentRequest
            {
                Id = id,
                MüşteriAdı = appointment.MüşteriAdı,
                MüşteriTelefon = appointment.MüşteriTelefon,
                HizmetId = appointment.HizmetId,
                RandevuTarihi = appointment.RandevuTarihi,
                RandevuSaati = appointment.RandevuSaati,
                Durum = "İptal",
                Notlar = appointment.Notlar
            };

            var result = await _appointmentService.UpdateAsync(updateRequest);
            
            return Ok(new ApiResponse<AppointmentResponse>
            {
                Success = true,
                Message = "Randevunuz başarıyla iptal edildi",
                Data = result
            });
        }
        catch (NotFoundException ex)
        {
            _logger.LogWarning("Randevu bulunamadı: {error}", ex.Message);
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
            _logger.LogError("Randevu iptal hatası: {error}", ex.Message);
            return StatusCode(500, new ApiResponse<object>
            {
                Success = false,
                Message = "Randevu iptal edilirken hata oluştu"
            });
        }
    }
}
