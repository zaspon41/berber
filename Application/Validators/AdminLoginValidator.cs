namespace Application.Validators;

using FluentValidation;
using Application.DTOs;

public class AdminLoginValidator : AbstractValidator<AdminLoginRequest>
{
    public AdminLoginValidator()
    {
        RuleFor(x => x.AdminUserName)
            .NotEmpty().WithMessage("Admin kullanıcı adı boş bırakılamaz")
            .MinimumLength(3).WithMessage("Admin kullanıcı adı en az 3 karakter olmalıdır")
            .MaximumLength(100).WithMessage("Admin kullanıcı adı en fazla 100 karakter olmalıdır");

        RuleFor(x => x.AdminPassword)
            .NotEmpty().WithMessage("Admin şifresi boş bırakılamaz")
            .MinimumLength(3).WithMessage("Admin şifresi en az 3 karakter olmalıdır")
            .MaximumLength(100).WithMessage("Admin şifresi en fazla 100 karakter olmalıdır");
    }
}
