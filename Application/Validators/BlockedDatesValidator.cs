namespace Application.Validators;

using System;
using Application.DTOs;
using FluentValidation;

public class CreateBlockedDatesValidator : AbstractValidator<CreateBlockedDatesRequest>
{
    public CreateBlockedDatesValidator()
    {
        RuleFor(x => x.Tarih)
            .NotEmpty().WithMessage("Tarih boş olamaz")
            .GreaterThanOrEqualTo(DateOnly.FromDateTime(DateTime.Today)).WithMessage("Tarih bugün veya sonrası olmalıdır");

        RuleFor(x => x.Neden)
            .MaximumLength(200).WithMessage("Neden maksimum 200 karakter olabilir");
    }
}

public class UpdateBlockedDatesValidator : AbstractValidator<UpdateBlockedDatesRequest>
{
    public UpdateBlockedDatesValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Geçerli ID gereklidir");

        RuleFor(x => x.Tarih)
            .NotEmpty().WithMessage("Tarih boş olamaz");

        RuleFor(x => x.Neden)
            .MaximumLength(200).WithMessage("Neden maksimum 200 karakter olabilir");
    }
}
