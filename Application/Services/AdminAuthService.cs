namespace Application.Services;

using System.Linq;
using System.Threading.Tasks;
using Application.DTOs;
using Application.Exceptions;
using Application.Interfaces;
using FluentValidation;

public class AdminAuthService : IAdminService
{
    private readonly IAdminAuthRepository _adminAuthRepository;
    private readonly IValidator<AdminLoginRequest> _validator;

    public AdminAuthService(IAdminAuthRepository adminAuthRepository, IValidator<AdminLoginRequest> validator)
    {
        _adminAuthRepository = adminAuthRepository;
        _validator = validator;
    }

    public async Task<AdminLoginResponse> LoginAsync(AdminLoginRequest request)
    {
        var validationResult = await _validator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
            throw new Application.Exceptions.ValidationException(errors);
        }

        var admin = await _adminAuthRepository.GetByCredentialsAsync(request.AdminUserName, request.AdminPassword);

        if (admin == null)
        {
            throw new UnauthorizedException("Kullanıcı adı veya şifre yanlış");
        }

        return new AdminLoginResponse
        {
            Id = admin.Id,
            AdminUserName = admin.AdminUserName,
            Message = "Başarıyla giriş yapıldı"
        };
    }
}
